// ── Activity Page JS ─────────────────────────────────────────────────────────

let allTrainersData = [];

async function loadActivities() {
  const container   = document.getElementById("activities");
  const filterSelect = document.getElementById("filterSelect");

  container.innerHTML = `
    <div class="col-12 text-center py-5">
      <div class="spinner-border" style="color:var(--clubly-accent)" role="status"></div>
      <p class="mt-3 text-muted">Loading activities…</p>
    </div>`;

  try {
    const [actRes, trRes] = await Promise.all([
      fetch("http://clublywebsite.runasp.net/api/Activities"),
      fetch("http://clublywebsite.runasp.net/api/Trainers")
    ]);

    const data     = actRes.ok ? await actRes.json() : [];
    const trainers = trRes.ok  ? await trRes.json()  : [];

    // Active trainers only
    allTrainersData = trainers.filter(t => t.IsActive === true || t.isActive === true);

    // Active activities only
    const active = data.filter(a => {
      if (a.IsActive !== undefined) return a.IsActive === true;
      if (a.isActive !== undefined) return a.isActive === true;
      if (a.Status   !== undefined) return (a.Status+'').toLowerCase() === 'active';
      if (a.status   !== undefined) return (a.status+'').toLowerCase() === 'active';
      return true;
    });

    // ── Populate facility filter dynamically ─────────────────────────────────
    if (filterSelect) {
      const facilities = [...new Set(active.map(a => a.FacilityName || a.facilityName).filter(Boolean))].sort();
      filterSelect.innerHTML = `<option value="">All Facilities</option>`;
      facilities.forEach(f => {
        filterSelect.innerHTML += `<option value="${f.toLowerCase()}">${f}</option>`;
      });
    }

    if (active.length === 0) {
      container.innerHTML = `<p class="col-12 text-center text-muted py-5">No activities available right now.</p>`;
      return;
    }

    window._allActivities = active;
    renderActivities(active);

  } catch (err) {
    console.error("Error loading activities:", err);
    const isNet = err instanceof TypeError && err.message.includes('fetch');
    container.innerHTML = `<p class="col-12 text-center text-danger py-4">${isNet
      ? '⚠️ Could not connect to the server. Make sure the backend is running on localhost:7132.'
      : '⚠️ Failed to load activities: ' + err.message}</p>`;
  }
}

// ── Render activity cards ─────────────────────────────────────────────────────
function renderActivities(list) {
  const container = document.getElementById("activities");
  if (!list || list.length === 0) {
    container.innerHTML = `<p class="col-12 text-center text-muted py-4">No activities found.</p>`;
    return;
  }

  container.innerHTML = list.map(a => {
    const id      = a.Id          || a.id;
    const name    = a.Name        || a.name        || '';
    const desc    = a.Description || a.description || '';
    const price   = a.Price       ?? a.price       ?? null;
    const facName = a.FacilityName || a.facilityName || '';
    const imgUrl  = a.ImageUrl    || a.imageUrl    || null;

    const imgStyle = imgUrl
      ? `background-image:url('https://localhost:7132${imgUrl}');`
      : `background:linear-gradient(135deg,#0d1b2a,#1e3a5f);`;

    const priceBadge = price != null
      ? `<span class="activity-price-badge"><i class="bi bi-tag-fill me-1"></i>${price} EGP</span>` : '';
    const facBadge = facName
      ? `<span class="activity-facility-badge"><i class="bi bi-building me-1"></i>${facName}</span>` : '';

    const actTrainers  = getTrainersForActivity(id);
    const trainerCount = actTrainers.length;
    const trainerBadge = trainerCount > 0
      ? `<span class="activity-trainer-count"><i class="bi bi-person-fill me-1"></i>${trainerCount} Trainer${trainerCount>1?'s':''}</span>` : '';

    const viewBtn = trainerCount > 0
      ? `<a class="btn-view-trainers" href="trainers.html#activity-${id}">
           <i class="bi bi-people-fill"></i> View Trainers
         </a>` : '';

    return `
    <div class="col-md-6 col-lg-4 activity-item mb-4"
         data-id="${id}"
         data-name="${name.toLowerCase()}"
         data-facility="${facName.toLowerCase()}"
         data-price="${price ?? 0}">
      <div class="activity-card" style="${imgStyle}">
        <div class="activity-overlay">
          <div class="activity-badges">${priceBadge}${facBadge}${trainerBadge}</div>
          <h3>${name}</h3>
          <p>${desc}</p>
          <div class="d-flex gap-2 flex-wrap">
            <button class="btn-custom" onclick="handleBookActivity(${id})">
              <i class="bi bi-calendar-plus"></i> Book Now
            </button>
            ${viewBtn}
          </div>
        </div>
      </div>
    </div>`;
  }).join('');
}

// ── Match trainers by ActivityIds ─────────────────────────────────────────────
function getTrainersForActivity(activityId) {
  return allTrainersData.filter(t => {
    const ids = t.ActivityIds || t.activityIds || [];
    return ids.includes(activityId) || ids.includes(Number(activityId));
  });
}

// ── Filters ───────────────────────────────────────────────────────────────────
function applyActivityFilters() {
  const search   = (document.getElementById("searchInput")?.value  || '').toLowerCase().trim();
  const facVal   = (document.getElementById("filterSelect")?.value || '').toLowerCase().trim();
  const priceVal =  document.getElementById("priceFilter")?.value  || '';

  let found = 0;
  document.querySelectorAll(".activity-item").forEach(item => {
    const name     = item.dataset.name     || '';
    const facility = item.dataset.facility || '';
    const price    = parseInt(item.dataset.price || '0');

    let show = (!search || name.includes(search))
            && (!facVal || facility === facVal);

    if (priceVal === 'low')  show = show && price <= 100;
    if (priceVal === 'mid')  show = show && price > 100 && price <= 300;
    if (priceVal === 'high') show = show && price > 300;

    item.style.display = show ? '' : 'none';
    if (show) found++;
  });

  // No results message
  let noMsg = document.getElementById('act-no-result');
  const grid = document.getElementById('activities');
  if (found === 0 && (search || facVal || priceVal)) {
    if (!noMsg && grid) {
      noMsg = document.createElement('p');
      noMsg.id = 'act-no-result';
      noMsg.className = 'col-12 text-center text-muted py-5';
      grid.appendChild(noMsg);
    }
    if (noMsg) noMsg.innerHTML = `<i class="bi bi-search me-2"></i>No activities found.`;
  } else {
    noMsg?.remove();
  }
}

function clearActivityFilters() {
  ['searchInput','filterSelect','priceFilter'].forEach(id => {
    const el = document.getElementById(id);
    if (el) el.value = '';
  });
  applyActivityFilters();
}

function handleBookActivity(id) {
  window._currentActivityId = id;
  const act  = (window._allActivities || []).find(a => (a.Id || a.id) == id);
  const name = act ? (act.Name || act.name || 'Activity') : 'Activity';
  openActivityGroupPicker(id, name);
}

async function openActivityGroupPicker(activityId, activityName) {
  // Set modal title
  document.getElementById('actPickerTitle').textContent = activityName;
  document.getElementById('actPickerBody').innerHTML = `
    <div class="text-center py-4">
      <div class="spinner-border spinner-border-sm" style="color:#e85d2f"></div>
      <p class="mt-2 small text-muted">Loading groups…</p>
    </div>`;
onclick="selectActivitySlot('${btnId}',${gid},'${encodeURIComponent(gname)}','${fromDate}','','',${price},'${encodeURIComponent(trainer)}',${g.durationDays||0},${g.trainerId||g.TrainerId||'null'})"

  const confirmBtn = document.getElementById('actPickerConfirm');
  confirmBtn.style.opacity = '.4';
  confirmBtn.style.pointerEvents = 'none';
  document.getElementById('actPickerSelected').textContent = '';

  window._actPickerChoice = null;
  window._actPickerActivityName = activityName;

  new bootstrap.Modal(document.getElementById('actPickerModal')).show();

  try {
const res  = await fetch('http://clublywebsite.runasp.net/api/ActivityGroups');
    const all  = res.ok ? await res.json() : [];

  const groups = all.filter(g => {
  const aid    = g.activityId || g.ActivityId;
  const status = (g.status || g.Status || '').toLowerCase();
  return String(aid) === String(activityId) && status === 'active';
});

    if (!groups.length) {
      document.getElementById('actPickerBody').innerHTML = `
        <div class="text-center py-5">
          <i class="bi bi-calendar-x" style="font-size:2.5rem;color:#94a3b8"></i>
          <p class="mt-3 text-muted">No available groups for this activity right now.</p>
        </div>`;
      return;
    }

    let html = '';
    groups.forEach(g => {
      const gid      = g.id || g.Id;
      const gname    = g.name || g.Name || '';
      const trainer  = g.trainerName || g.TrainerName || '';
      const price    = g.price ?? g.Price ?? 0;
      const capacity = g.capacity || g.Capacity || '';
      const slots    = g.timeSlots || g.TimeSlots || [];

    // حساب أول وآخر تاريخ
const dates = slots.map(sl => sl.date || sl.Date || '').filter(Boolean).sort();
const fromDate = dates[0] || '';
const toDate   = dates[dates.length - 1] || '';
const dateRange = fromDate === toDate
  ? fromDate
  : `${fromDate} → ${toDate}`;

const btnId = `actgrp_${gid}`;
html += `
<button class="act-slot-btn" id="${btnId}"
onclick="selectActivitySlot('${btnId}',${gid},'${encodeURIComponent(gname)}','${fromDate}','','',${price},'${encodeURIComponent(trainer)}',${g.durationDays||0},${g.trainerId||g.TrainerId||'null'})"  style="width:100%;text-align:left;border:2px solid var(--border,#e2e8f0);background:var(--bg,#f0f4f8);border-radius:14px;padding:14px 16px;cursor:pointer;font-family:'Cairo',sans-serif;transition:all .18s;">
  <div style="display:flex;align-items:center;justify-content:space-between;margin-bottom:8px;">
    <span style="font-size:.95rem;font-weight:900;color:var(--text,#1a202c);">${gname}</span>
    ${price > 0
      ? `<span style="font-size:.82rem;font-weight:800;color:#fff;background:#2ec4b6;padding:3px 12px;border-radius:20px;">${price} EGP</span>`
      : `<span style="font-size:.78rem;color:#94a3b8;font-weight:700;">Free</span>`}
  </div>
  <div style="font-size:.78rem;color:var(--muted,#64748b);display:flex;flex-wrap:wrap;gap:10px;">
    ${trainer ? `<span><i class="bi bi-person-fill" style="color:#e85d2f"></i> ${trainer}</span>` : ''}
    ${capacity ? `<span><i class="bi bi-people-fill" style="color:#2ec4b6"></i> Capacity: ${capacity}</span>` : ''}
${g.durationDays ? `<span><i class="bi bi-hourglass-split" style="color:#7c3aed"></i> ${g.durationDays} days</span>` : ''}
  </div>
  ${slots.length ? `
  <div style="display:flex;flex-wrap:wrap;gap:6px;margin-top:10px;">
    ${slots.map(sl => {
      const date  = (sl.date  || sl.Date  || '').split('T')[0];
      const start = (sl.startTime || sl.StartTime || '').substring(0, 5);
      const end   = (sl.endTime   || sl.EndTime   || '').substring(0, 5);
const dayName = date ? new Date(date + 'T00:00:00').toLocaleDateString('en-GB', {weekday:'long'}) : '';
const day = sl.day || sl.Day || '';
return `<span style="font-size:.75rem;font-weight:700;padding:4px 10px;border-radius:20px;background:rgba(232,93,47,.08);color:#e85d2f;border:1px solid rgba(232,93,47,.2);">
  <i class="bi bi-clock" style="font-size:.65rem"></i> ${day ? day+' · ' : ''} ${start} → ${end}
</span>`;
    }).join('')}
  </div>` : ''}
</button>`;  

    });

    document.getElementById('actPickerBody').innerHTML = html;

  } catch(e) {
    document.getElementById('actPickerBody').innerHTML = `
      <div class="text-center py-4 text-danger">
        <i class="bi bi-exclamation-triangle me-1"></i>Failed to load groups.
      </div>`;
  }
}

function selectActivitySlot(btnId, groupId, groupName, date, startTime, endTime, price, trainer, durationDays, trainerId) {
    document.querySelectorAll('.act-slot-btn').forEach(btn => {
    btn.style.borderColor = 'var(--border,#e2e8f0)';
    btn.style.background  = 'var(--bg,#f0f4f8)';
    btn.style.color       = 'var(--text,#1a202c)';
    btn.style.boxShadow   = '';
  });
  const btn = document.getElementById(btnId);
  if (btn) {
    btn.style.borderColor = '#e85d2f';
    btn.style.background  = 'rgba(232,93,47,.08)';
    btn.style.color       = '#e85d2f';
    btn.style.boxShadow   = '0 0 0 3px rgba(232,93,47,.15)';
  }
window._actPickerChoice = { 
  groupId, groupName, date, startTime, endTime, price, trainer, durationDays,
  activityId: window._currentActivityId || 0,
  trainerId: trainerId || null 
};  document.getElementById('actPickerSelected').innerHTML =
    `<i class="bi bi-check-circle-fill" style="color:#2ec4b6"></i> <strong>${groupName}</strong> &nbsp; ${date} &nbsp; ${startTime} → ${endTime}`;

  const confirmBtn = document.getElementById('actPickerConfirm');
  confirmBtn.style.opacity = '1';
  confirmBtn.style.pointerEvents = 'auto';
}

function actPickerConfirm() {
  if (!window._actPickerChoice) return;
  bootstrap.Modal.getInstance(document.getElementById('actPickerModal')).hide();

  const c = window._actPickerChoice;
    window._actPickerActivityId = c.activityId || window._currentActivityId || 0;
 window._actPickerTrainerId  = c.trainerId || null;
  openBookingModal('activity', c.groupId, window._actPickerActivityName, c.price);

 setTimeout(() => {
    const c = window._actPickerChoice;
    const dateEl = document.getElementById('bpDate');
    const timeEl = document.getElementById('bpTime');
    const endEl  = document.getElementById('bpEndTime');

    // default date = today
    const today = new Date().toISOString().split('T')[0];
    if (dateEl) dateEl.value = today;
    if (timeEl) timeEl.value = c.startTime || '';
    if (endEl)  endEl.value  = c.endTime   || '';

    // إضافة end date field إذا في durationDays
    if (c.durationDays) {
      const endDate = new Date();
      endDate.setDate(endDate.getDate() + c.durationDays);
      const endDateStr = endDate.toISOString().split('T')[0];
      // ابحث عن حقل End Time وضيف بعده End Date
      const endField = endEl?.closest('.bp-field');
  const endDateEl = document.getElementById('bpEndDate');
const endDateWrap = document.getElementById('bpEndDateWrap');
if (endDateEl && endDateWrap) {
  endDateEl.value = endDateStr;
  endDateWrap.style.display = '';
  // إضافة الـ badge
  const parent = endDateEl.closest('.bp-field');
  if (parent && !parent.querySelector('.slot-lock-badge')) {
    const badge = document.createElement('div');
    badge.className = 'slot-lock-badge';
    badge.innerHTML = `<i class="bi bi-lock-fill" style="font-size:.6rem"></i> Calculated from duration (${c.durationDays} days)`;
    badge.style.cssText = 'font-size:.68rem;font-weight:700;color:#e85d2f;margin-top:3px;display:flex;align-items:center;gap:3px;';
    parent.appendChild(badge);
  }

      }
    }

    [dateEl, timeEl, endEl].forEach(el => {
      if (!el) return;
      el.setAttribute('readonly', true);
      el.style.background = 'var(--bg,#f0f4f8)';
      el.style.color      = 'var(--muted,#64748b)';
      el.style.cursor     = 'not-allowed';
      el.style.pointerEvents = 'none';
      const parent = el.closest('.bp-field');
      if (parent && !parent.querySelector('.slot-lock-badge')) {
        const badge = document.createElement('div');
        badge.className = 'slot-lock-badge';
        badge.innerHTML = `<i class="bi bi-lock-fill" style="font-size:.6rem"></i> Fixed from group`;
        badge.style.cssText = 'font-size:.68rem;font-weight:700;color:#e85d2f;margin-top:3px;display:flex;align-items:center;gap:3px;';
        parent.appendChild(badge);
      }
    });

    const paxEl = document.getElementById('bpParticipants');
    if (paxEl) { paxEl.removeAttribute('disabled'); paxEl.focus(); }
  }, 380);
}

// ── Init ──────────────────────────────────────────────────────────────────────
document.addEventListener('DOMContentLoaded', () => {
  loadActivities();
  document.getElementById("searchInput")?.addEventListener("input",  applyActivityFilters);
  document.getElementById("filterSelect")?.addEventListener("change", applyActivityFilters);
  document.getElementById("priceFilter")?.addEventListener("change",  applyActivityFilters);
});


function renderActivityStars(r) {
  return [1,2,3,4,5].map(i =>
    r>=i ? '<i class="bi bi-star-fill"></i>' : r>=i-.5 ? '<i class="bi bi-star-half"></i>' : '<i class="bi bi-star"></i>'
  ).join('');
}
