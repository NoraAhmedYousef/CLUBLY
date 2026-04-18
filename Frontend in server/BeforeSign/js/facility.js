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
    if (!facRes.ok || !catRes.ok) throw new Error("Failed to fetch data");

    allFacilities = await facRes.json();
    allCategories = await catRes.json();

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
    if (errorMsg) errorMsg.textContent = 'Failed to load facilities. Please try again later.';
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

// ── Toggle accordion (shared with trainers) ───────────────────────────────────
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

    // Auto-open if filter active and has results
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

// ── Book ──────────────────────────────────────────────────────────────────────
function handleBookFacility(id) {
  if (!localStorage.getItem('token')) { showSignInToast('facility'); return; }
  window.location.href = `booking.html?facility=${id}`;
}

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
    <span>Sign in to book a <strong style="color:#e85d2f;">${label}</strong></span>
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

loadFacilities();
