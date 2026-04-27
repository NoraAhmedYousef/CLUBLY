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
      fetch("https://clublywebsite.runasp.net/api/Activities"),
      fetch("https//clublywebsite.runasp.net/api/Trainers")
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
    container.innerHTML = `<p class="col-12 text-center text-danger py-4">Failed to load activities. Please try again later.</p>`;
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
      ? `background-image:url('https://clublywebsite.runasp.net${imgUrl}');`
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

// ── Book — requires sign-in ───────────────────────────────────────────────────
function handleBookActivity(id) {
  if (!localStorage.getItem('token')) { showSignInToast('activity'); return; }
  window.location.href = `booking.html?activity=${id}`;
}

// ── Init ──────────────────────────────────────────────────────────────────────
document.addEventListener('DOMContentLoaded', () => {
  loadActivities();
  document.getElementById("searchInput")?.addEventListener("input",  applyActivityFilters);
  document.getElementById("filterSelect")?.addEventListener("change", applyActivityFilters);
  document.getElementById("priceFilter")?.addEventListener("change",  applyActivityFilters);
});

// ── Sign-in Toast ─────────────────────────────────────────────────────────────
function showSignInToast(label) {
  let t = document.getElementById('signin-toast');
  if (!t) {
    t = document.createElement('div');
    t.id = 'signin-toast';
    t.style.cssText = `position:fixed;bottom:36px;left:50%;transform:translateX(-50%) translateY(20px);
      background:#0d1b2a;color:#fff;padding:16px 24px;border-radius:16px;
      box-shadow:0 8px 32px rgba(0,0,0,.4);z-index:99999;display:flex;
      align-items:center;gap:12px;font-family:'Cairo',sans-serif;font-size:.9rem;
      font-weight:600;opacity:0;transition:all .35s;max-width:92vw;`;
    document.body.appendChild(t);
  }
  t.innerHTML = `
    <i class="bi bi-lock-fill" style="font-size:1.3rem;color:#e85d2f;"></i>
    <span>Sign in to book an <strong style="color:#e85d2f;">${label}</strong></span>
    <button onclick="openModal('signin')"
      style="background:linear-gradient(135deg,#e85d2f,#c0392b);color:#fff;border:none;
             border-radius:50px;padding:7px 18px;font-family:'Cairo',sans-serif;
             font-weight:700;font-size:.82rem;cursor:pointer;white-space:nowrap;">Sign In</button>
    <button onclick="document.getElementById('signin-toast').style.opacity='0'"
      style="background:rgba(255,255,255,.1);color:#fff;border:none;border-radius:50%;
             width:26px;height:26px;cursor:pointer;font-size:15px;">✕</button>`;
  requestAnimationFrame(() => { t.style.opacity='1'; t.style.transform='translateX(-50%) translateY(0)'; });
  clearTimeout(t._timer);
  t._timer = setTimeout(() => { t.style.opacity='0'; t.style.transform='translateX(-50%) translateY(20px)'; }, 4500);
}

function renderActivityStars(r) {
  return [1,2,3,4,5].map(i =>
    r>=i ? '<i class="bi bi-star-fill"></i>' : r>=i-.5 ? '<i class="bi bi-star-half"></i>' : '<i class="bi bi-star"></i>'
  ).join('');
}
