// ── Facility Page JS ─────────────────────────────────────────────────────────

let allFacilities  = [];
let allCategories  = [];

async function loadFacilities() {
  const container = document.getElementById('categories-container');
  const errorMsg  = document.getElementById('error-msg');

  container.innerHTML = `
    <div class="text-center py-5">
      <div class="spinner-border" style="color:var(--clubly-accent)" role="status"></div>
      <p class="mt-3 text-muted">Loading facilities…</p>
    </div>`;

  try {
    const [facRes, catRes] = await Promise.all([
      fetch("http://clublywebsite.runasp.net/api/Facilities"),
      fetch("http://clublywebsite.runasp.net/api/FacilityCategories")
    ]);
    if (!facRes.ok) console.warn("Facilities API:", facRes.status);
    if (!catRes.ok)  console.warn("FacilityCategories API:", catRes.status);

    allFacilities = facRes.ok ? await facRes.json() : [];
    allCategories = catRes.ok ? await catRes.json() : [];

    // Filter: only active
    const active = allFacilities.filter(f => {
      if (f.IsActive  !== undefined) return f.IsActive  === true;
      if (f.isActive  !== undefined) return f.isActive  === true;
      if (f.Status    !== undefined) return (f.Status  + '').toLowerCase() === 'active';
      if (f.status    !== undefined) return (f.status  + '').toLowerCase() === 'active';
      return true;
    });

    // Populate category filter dynamically
    const catFilter = document.getElementById('categoryFilter');
    if (catFilter) {
      const usedCatIds = new Set(active.map(f => f.FacilityCategoryId || f.facilityCategoryId));
      allCategories.filter(c => usedCatIds.has(c.Id || c.id)).forEach(c => {
        catFilter.innerHTML += `<option value="${(c.Name||c.name||'').toLowerCase()}">${c.Name || c.name}</option>`;
      });
    }

    renderFacilitySections(allCategories, active);
    bindFacilityFilters();

  } catch (err) {
    console.error(err);
    container.innerHTML = '';
    if (errorMsg) {
      const isNet = err instanceof TypeError && err.message.includes('fetch');
      errorMsg.innerHTML = isNet
        ? '⚠️ Could not connect to the server. Make sure the backend is running on <strong>localhost:7132</strong>.'
        : `⚠️ Failed to load facilities: ${err.message}`;
    }
  }
}

// ── Render accordion sections ────────────────────────────────────────────────
function renderFacilitySections(categories, facilities) {
  const container = document.getElementById('categories-container');
  container.innerHTML = '';

  const usedCatIds  = new Set(facilities.map(f => f.FacilityCategoryId || f.facilityCategoryId));
  const visibleCats = categories.filter(c => usedCatIds.has(c.Id || c.id));

  if (visibleCats.length === 0) {
    container.innerHTML = `<p class="text-center text-muted py-5">No facilities available right now.</p>`;
    return;
  }

  const catIcons = {
    court: 'bi-dribbble', courts: 'bi-dribbble',
    gym: 'bi-activity', pool: 'bi-water', pools: 'bi-water',
    field: 'bi-geo-fill', track: 'bi-flag-fill'
  };

  visibleCats.forEach((cat, ci) => {
    const catId   = cat.Id   || cat.id;
    const catName = cat.Name || cat.name || '';
    const iconKey = catName.toLowerCase().split(' ')[0];
    const icon    = catIcons[iconKey] || 'bi-building';
    const isOpen  = ci === 0;

    const facInCat = facilities.filter(f =>
      (f.FacilityCategoryId || f.facilityCategoryId) === catId
    );

    const sectionId = `cat-section-${catId}`;
    const gridId    = `cat-grid-${catId}`;

    const cardsHtml = facInCat.map(fac => facilityCardHtml(fac, catName)).join('');

    container.innerHTML += `
    <div class="trainer-section facility-section ${isOpen ? 'is-open' : ''}"
         id="${sectionId}"
         data-catname="${catName.toLowerCase()}">
      <button class="trainer-section-header" onclick="toggleSection('${sectionId}')" aria-expanded="${isOpen}">
        <div class="trainer-section-icon">
          <i class="bi ${icon}"></i>
        </div>
        <div class="section-titles">
          <div class="trainer-section-label">Category</div>
          <div class="trainer-section-title">${catName}</div>
        </div>
        <div class="trainer-section-count">
          <i class="bi bi-building me-1"></i>${facInCat.length} Facilit${facInCat.length > 1 ? 'ies' : 'y'}
        </div>
        <i class="bi bi-chevron-down section-chevron"
           style="transition:transform .3s;${isOpen ? 'transform:rotate(180deg)' : ''}"></i>
      </button>
      <div class="trainer-section-grid" id="${gridId}" style="${isOpen ? '' : 'display:none'}">
        <div class="row g-4 pt-3 pb-2">
          ${cardsHtml}
        </div>
      </div>
    </div>`;
  });
}

// ── Single facility card HTML ─────────────────────────────────────────────────
function facilityCardHtml(fac, catName) {
  const fId     = fac.Id          || fac.id;
  const fName   = fac.Name        || fac.name        || '';
  const fDesc   = fac.Description || fac.description || '';
  const fCap    = fac.Capacity    || fac.capacity    || 0;
  const fImgUrl = fac.ImageUrl    || fac.imageUrl    || null;

  const imgHTML = fImgUrl
    ? `<img src="http://clublywebsite.runasp.net${fImgUrl}" alt="${fName}"
            onerror="this.parentElement.innerHTML='<div class=placeholder-img><i class=bi bi-image style=font-size:2rem;opacity:.4></i></div>'">`
    : `<div class="placeholder-img"><i class="bi bi-image" style="font-size:2rem;opacity:.4;"></i></div>`;

  return `
  <div class="col-md-4 facility-item"
       data-name="${fName.toLowerCase()}"
       data-capacity="${fCap}"
       data-category="${catName.toLowerCase()}">
    <div class="facility-card">
      <div class="facility-image">${imgHTML}</div>
      <div class="facility-info">
        <h5 class="facility-name">${fName}</h5>
        <p class="facility-description">${fDesc}</p>
        <div class="facility-meta-row">
          <span class="facility-meta-badge capacity-badge">
            <i class="bi bi-people-fill me-1"></i>Capacity: ${fCap}
          </span>
          <span class="facility-meta-badge category-badge">
            <i class="bi bi-grid-fill me-1"></i>${catName}
          </span>
        </div>
        <button class="btn-book-facility mt-3" onclick="handleBookFacility(${fId})">
          <i class="bi bi-calendar-plus me-1"></i>Book Now
        </button>
      </div>
    </div>
  </div>`;
}

// ── Toggle accordion ───────────────────────────────────────────────────────────
function toggleSection(id) {
  const section = document.getElementById(id);
  const grid    = section.querySelector('.trainer-section-grid');
  const chevron = section.querySelector('.section-chevron');
  const isOpen  = section.classList.contains('is-open');

  if (isOpen) {
    grid.style.maxHeight = grid.scrollHeight + 'px';
    requestAnimationFrame(() => {
      grid.style.transition = 'max-height .35s ease, opacity .3s ease';
      grid.style.maxHeight  = '0';
      grid.style.opacity    = '0';
    });
    setTimeout(() => { grid.style.display = 'none'; grid.style.maxHeight = ''; grid.style.opacity = ''; grid.style.transition = ''; }, 360);
    section.classList.remove('is-open');
    chevron.style.transform = 'rotate(0deg)';
    section.querySelector('.trainer-section-header').setAttribute('aria-expanded', 'false');
  } else {
    grid.style.display   = '';
    grid.style.maxHeight = '0';
    grid.style.opacity   = '0';
    requestAnimationFrame(() => {
      grid.style.transition = 'max-height .4s ease, opacity .35s ease';
      grid.style.maxHeight  = grid.scrollHeight + 'px';
      grid.style.opacity    = '1';
    });
    setTimeout(() => { grid.style.maxHeight = ''; grid.style.transition = ''; }, 420);
    section.classList.add('is-open');
    chevron.style.transform = 'rotate(180deg)';
    section.querySelector('.trainer-section-header').setAttribute('aria-expanded', 'true');
  }
}

// ── Bind filters ──────────────────────────────────────────────────────────────
function bindFacilityFilters() {
  ['searchInput','categoryFilter','capacityFilter'].forEach(id => {
    document.getElementById(id)?.addEventListener('input',  applyFacilityFilters);
    document.getElementById(id)?.addEventListener('change', applyFacilityFilters);
  });
}

function applyFacilityFilters() {
  const search  = (document.getElementById('searchInput')?.value    || '').toLowerCase().trim();
  const catVal  = (document.getElementById('categoryFilter')?.value || '').toLowerCase().trim();
  const capVal  =  document.getElementById('capacityFilter')?.value || '';

  document.querySelectorAll('.facility-section').forEach(section => {
    const items = section.querySelectorAll('.facility-item');
    let hits = 0;

    items.forEach(item => {
      const name   = item.dataset.name     || '';
      const cat    = item.dataset.category || '';
      const capNum = parseInt(item.dataset.capacity || '0');

      let show = (!search || name.includes(search))
              && (!catVal || cat.includes(catVal));

      if (capVal === 'small')  show = show && capNum < 50;
      if (capVal === 'medium') show = show && capNum >= 50 && capNum <= 100;
      if (capVal === 'large')  show = show && capNum > 100;

      item.style.display = show ? '' : 'none';
      if (show) hits++;
    });

    section.style.display = hits > 0 ? '' : 'none';

    if (hits > 0 && (search || catVal || capVal)) {
      const grid    = section.querySelector('.trainer-section-grid');
      const chevron = section.querySelector('.section-chevron');
      if (grid && grid.style.display === 'none') {
        grid.style.display = '';
        if (chevron) chevron.style.transform = 'rotate(180deg)';
        section.classList.add('is-open');
      }
    }
  });
}

function clearFacilityFilters() {
  ['searchInput','categoryFilter','capacityFilter'].forEach(id => {
    const el = document.getElementById(id);
    if (el) el.value = '';
  });
  applyFacilityFilters();
}

// ── Schedule Picker Modal ─────────────────────────────────────────────────────
function buildScheduleModal() {
  if (document.getElementById('schedPickerModal')) return;
  const el = document.createElement('div');
  el.innerHTML = `
  <div class="modal fade" id="schedPickerModal" tabindex="-1">
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable" style="max-width:520px">
      <div class="modal-content" style="border-radius:20px;border:none;overflow:hidden;background:var(--card-bg,#fff);color:var(--text,#1a202c);">
        <div style="background:linear-gradient(135deg,#0d1b2a,#122236);padding:20px 24px 16px;display:flex;align-items:center;justify-content:space-between;">
          <div>
            <div style="font-size:.7rem;font-weight:700;text-transform:uppercase;letter-spacing:1px;color:rgba(255,255,255,.5);margin-bottom:3px;"><i class="bi bi-calendar3 me-1"></i>Choose a Time Slot</div>
            <h5 id="schedPickerTitle" style="margin:0;font-weight:900;color:#fff;font-size:1.1rem;"></h5>
          </div>
          <button type="button" data-bs-dismiss="modal" style="background:rgba(255,255,255,.1);border:none;width:34px;height:34px;border-radius:50%;cursor:pointer;color:#fff;font-size:16px;display:flex;align-items:center;justify-content:center;">✕</button>
        </div>
        <div class="modal-body" style="padding:20px 24px;" id="schedPickerBody">
          <div class="text-center py-4">
            <div class="spinner-border spinner-border-sm" style="color:#e85d2f"></div>
          </div>
        </div>
        <div class="modal-footer" style="border-top:1px solid var(--border,#e2e8f0);padding:14px 24px;display:flex;justify-content:space-between;align-items:center;">
          <span id="schedPickerSelected" style="font-size:.82rem;color:var(--muted,#64748b);"></span>
          <button id="schedPickerConfirm" onclick="schedPickerConfirm()"
            style="background:linear-gradient(135deg,#e85d2f,#c0392b);color:#fff;border:none;border-radius:50px;padding:9px 28px;font-weight:700;font-family:'Cairo',sans-serif;font-size:.88rem;cursor:pointer;opacity:.4;pointer-events:none;transition:opacity .2s;">
            <i class="bi bi-calendar-check me-1"></i>Confirm Slot
          </button>
        </div>
      </div>
    </div>
  </div>`;
  document.body.appendChild(el.firstElementChild);
}

let _schedPickerChoice = null;

async function openSchedulePicker(facilityId, facilityName) {
  buildScheduleModal();
  _schedPickerChoice = null;

  document.getElementById('schedPickerTitle').textContent = facilityName;
  document.getElementById('schedPickerSelected').textContent = '';
  const confirmBtn = document.getElementById('schedPickerConfirm');
  confirmBtn.style.opacity = '.4';
  confirmBtn.style.pointerEvents = 'none';
  document.getElementById('schedPickerBody').innerHTML = `
    <div class="text-center py-4">
      <div class="spinner-border spinner-border-sm" style="color:#e85d2f"></div>
      <p class="mt-2 small" style="color:var(--muted,#64748b)">Loading available slots…</p>
    </div>`;

  const modal = new bootstrap.Modal(document.getElementById('schedPickerModal'));
  modal.show();

  try {
    const res  = await fetch('http://clublywebsite.runasp.net/api/FacilitySchedules');
    const all  = res.ok ? await res.json() : [];

    const scheds = all.filter(s => {
      const fid    = s.facilityId || s.FacilityId;
      const status = (s.status || s.Status || '').toLowerCase();
      return String(fid) === String(facilityId) && status === 'active';
    });

    if (!scheds.length) {
      document.getElementById('schedPickerBody').innerHTML = `
        <div class="text-center py-5">
          <i class="bi bi-calendar-x" style="font-size:2.5rem;color:var(--muted,#94a3b8)"></i>
          <p class="mt-3" style="color:var(--muted,#64748b)">No available schedules for this facility right now.</p>
        </div>`;
      return;
    }

    // Group by date
    const grouped = {};
    scheds.forEach(s => {
      const date = (s.date || s.Date || '').split('T')[0];
      if (!grouped[date]) grouped[date] = [];
      grouped[date].push(s);
    });

    let html = '';
    Object.keys(grouped).sort().forEach(date => {
      const d = new Date(date + 'T00:00:00');
      const dateLabel = d.toLocaleDateString('en-GB', { weekday:'long', day:'2-digit', month:'short', year:'numeric' });
      html += `
      <div style="margin-bottom:20px;">
        <div style="font-size:.72rem;font-weight:700;text-transform:uppercase;letter-spacing:.8px;color:var(--muted,#64748b);margin-bottom:10px;padding-bottom:6px;border-bottom:1px solid var(--border,#e2e8f0);">
          <i class="bi bi-calendar3 me-1" style="color:#e85d2f"></i>${dateLabel}
        </div>
        <div style="display:flex;flex-wrap:wrap;gap:8px;">`;

      grouped[date].forEach(s => {
        const slots = s.timeSlots || s.TimeSlots || [];
        const day   = s.day || s.Day || '';
        slots.forEach(sl => {
          const start  = (sl.startTime || sl.StartTime || '').substring(0, 5);
          const end    = (sl.endTime   || sl.EndTime   || '').substring(0, 5);
          const slotId = `slot_${s.id || s.Id}_${start.replace(':', '')}`;
          html += `
          <button class="sched-slot-btn" id="${slotId}"
onclick="selectScheduleSlot('${slotId}',${s.id||s.Id},${facilityId},'${date}','${day}','${start}','${end}',${sl.price||sl.Price||0})"            style="border:2px solid var(--border,#e2e8f0);background:var(--bg,#f0f4f8);border-radius:12px;padding:9px 16px;cursor:pointer;font-family:'Cairo',sans-serif;font-size:.82rem;font-weight:700;color:var(--text,#1a202c);transition:all .18s;display:inline-flex;align-items:center;gap:6px;">
            <i class="bi bi-clock" style="color:#e85d2f;font-size:.8rem"></i>
            ${start} → ${end}
${day ? `<span style="font-size:.68rem;font-weight:600;color:var(--muted,#64748b)">${day}</span>` : ''}
${(sl.price||sl.Price||0) > 0 ? `<span style="font-size:.72rem;font-weight:800;color:#2ec4b6;margin-left:4px;">${sl.price||sl.Price} EGP</span>` : `<span style="font-size:.68rem;color:#94a3b8">Free</span>`}          </button>`;
        });
      });

      html += `</div></div>`;
    });

    document.getElementById('schedPickerBody').innerHTML = html;

  } catch(e) {
    document.getElementById('schedPickerBody').innerHTML = `
      <div class="text-center py-4" style="color:#e53e3e">
        <i class="bi bi-exclamation-triangle me-1"></i>Failed to load schedules. Please try again.
      </div>`;
  }
}

function selectScheduleSlot(slotId, scheduleId, facilityId, date, day, startTime, endTime, slotPrice = 0) {  document.querySelectorAll('.sched-slot-btn').forEach(btn => {
    btn.style.borderColor  = 'var(--border,#e2e8f0)';
    btn.style.background   = 'var(--bg,#f0f4f8)';
    btn.style.color        = 'var(--text,#1a202c)';
    btn.style.boxShadow    = '';
  });
  const btn = document.getElementById(slotId);
  if (btn) {
    btn.style.borderColor = '#e85d2f';
    btn.style.background  = 'rgba(232,93,47,.08)';
    btn.style.color       = '#e85d2f';
    btn.style.boxShadow   = '0 0 0 3px rgba(232,93,47,.15)';
  }
_schedPickerChoice = { scheduleId, facilityId, date, day, startTime, endTime, slotPrice };
  document.getElementById('schedPickerSelected').innerHTML =
    `<i class="bi bi-check-circle-fill" style="color:#2ec4b6"></i> <strong>${date}</strong> &nbsp;${startTime} → ${endTime}`;

  const confirmBtn = document.getElementById('schedPickerConfirm');
  confirmBtn.style.opacity = '1';
  confirmBtn.style.pointerEvents = 'auto';
}

// ── Lock a field visually and functionally ────────────────────────────────────
function lockField(el) {
  if (!el) return;
  el.setAttribute('readonly', 'true');
  el.setAttribute('disabled', 'true');
  el.style.background    = 'var(--bg,#f0f4f8)';
  el.style.color         = 'var(--muted,#64748b)';
  el.style.cursor        = 'not-allowed';
  el.style.pointerEvents = 'none';
  el.style.opacity       = '0.85';
  el.style.borderColor   = 'var(--border,#e2e8f0)';
}

function addLockBadge(el) {
  if (!el) return;
  const parent = el.closest('.mb-3,.col,.form-group') || el.parentElement;
  if (!parent || parent.querySelector('.slot-lock-badge')) return;
  const badge = document.createElement('div');
  badge.className = 'slot-lock-badge';
  badge.innerHTML = `<i class="bi bi-lock-fill" style="font-size:.6rem"></i> Fixed from schedule`;
  badge.style.cssText = 'font-size:.68rem;font-weight:700;color:#e85d2f;margin-top:3px;display:flex;align-items:center;gap:3px;';
  parent.appendChild(badge);
}

// ── Confirm chosen slot → open booking modal with locked fields ───────────────
function schedPickerConfirm() {
  if (!_schedPickerChoice) return;
  bootstrap.Modal.getInstance(document.getElementById('schedPickerModal')).hide();

  const fac  = (allFacilities || []).find(f => (f.Id || f.id) == _schedPickerChoice.facilityId);
  const name = fac ? (fac.Name || fac.name || 'Facility') : 'Facility';

openBookingModal('facility', _schedPickerChoice.facilityId, name, _schedPickerChoice.slotPrice || 0, [], [], _schedPickerChoice.scheduleId);
 setTimeout(() => {
    const dateEl     = document.getElementById('bpDate');
    const timeEl     = document.getElementById('bpTime');
    const facilityEl = document.getElementById('bpFacility') || document.getElementById('bpName');
    const paxEl      = document.getElementById('bpParticipants') || document.getElementById('bpPax');

    // ── Fill start date & time ──
    if (dateEl) { dateEl.value = _schedPickerChoice.date; dateEl.dispatchEvent(new Event('input')); }
    if (timeEl) { timeEl.value = _schedPickerChoice.startTime; timeEl.dispatchEvent(new Event('input')); }

    // ── Fill end time (now a native field in facilityForm) ──
    const endTimeEl = document.getElementById('bpEndTime');
    if (endTimeEl) {
      endTimeEl.value = _schedPickerChoice.endTime;
      lockField(endTimeEl);
      addLockBadge(endTimeEl);
    }

    // ── Lock all fields except participants ──
    lockField(dateEl);
    lockField(timeEl);
    lockField(facilityEl);
    addLockBadge(dateEl);
    addLockBadge(timeEl);

    // ── Unlock & focus participants ──
    if (paxEl) {
      paxEl.removeAttribute('disabled');
      paxEl.removeAttribute('readonly');
      paxEl.style.cssText = '';
      paxEl.focus();
    }
  }, 380);
}

// ── Book ──────────────────────────────────────────────────────────────────────
function handleBookFacility(id) {
  const fac  = (allFacilities || []).find(f => (f.Id || f.id) == id);
  const name = fac ? (fac.Name || fac.name || 'Facility') : 'Facility';
  openSchedulePicker(id, name);
}


loadFacilities();
