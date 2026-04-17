// ── Trainer Page JS ─────────────────────────────────────────────────────────
const TRAINER_API  = 'https://localhost:7132/api/Trainers';
const ACTIVITY_API = 'https://localhost:7132/api/Activities';

document.addEventListener('DOMContentLoaded', () => {
  loadTrainers();
  window.addEventListener('scroll', () => {
    document.getElementById('mainHeader')?.classList.toggle('scrolled', window.scrollY > 50);
    document.getElementById('backTop')?.classList.toggle('show', window.scrollY > 300);
  });
});

// ── Load ──────────────────────────────────────────────────────────────────────
async function loadTrainers() {
  const container = document.getElementById('activities-container');
  const errorMsg  = document.getElementById('error-msg');

  container.innerHTML = `
    <div class="text-center py-5">
      <div class="spinner-border" style="color:var(--clubly-accent)" role="status"></div>
      <p class="mt-3 text-muted">Loading trainers…</p>
    </div>`;

  try {
    const [trRes, actRes] = await Promise.all([fetch(TRAINER_API), fetch(ACTIVITY_API)]);
    const trainers   = trRes.ok  ? await trRes.json()  : [];
    const activities = actRes.ok ? await actRes.json() : [];

    const activeTrainers = trainers.filter(t => t.IsActive === true || t.isActive === true);
    const activeActs     = activities.filter(a => {
      if (a.IsActive !== undefined) return a.IsActive === true;
      if (a.isActive !== undefined) return a.isActive === true;
      if (a.Status   !== undefined) return (a.Status+'').toLowerCase() === 'active';
      if (a.status   !== undefined) return (a.status+'').toLowerCase() === 'active';
      return true;
    });

    if (activeTrainers.length === 0) {
      container.innerHTML = `<p class="text-center text-muted py-5">No trainers available right now.</p>`;
      return;
    }
 // ✅ جلب الـ ratings هنا — جوه loadTrainers اللي هي async
    const ratingsResults = await Promise.allSettled(
      activeTrainers.map(t =>
        fetch(`https://localhost:7132/api/TrainerRatings/trainer/${t.Id || t.id}`)
          .then(r => r.ok ? r.json() : null)
          .catch(() => null)
      )
    );

    ratingsResults.forEach((result, i) => {
      if (result.status === 'fulfilled' && result.value) {
        activeTrainers[i].rating = result.value.averageRating ?? null;
      }
    });
    window._trainersData  = activeTrainers;
    window._activitiesData = activeActs;

    // Populate activity filter dropdown
    const actFilter = document.getElementById('activityFilter');
    if (actFilter) {
      const actNames = [...new Set(activeTrainers
        .map(t => t.Activities || t.activities)
        .filter(Boolean)
        .flatMap(a => a.split(',').map(s => s.trim()))
      )].sort();
      actNames.forEach(name => {
        actFilter.innerHTML += `<option value="${name.toLowerCase()}">${name}</option>`;
      });
    }

    renderSections(activeActs, activeTrainers);
    bindFilters();

    // Scroll to anchor
    const hash = window.location.hash;
    if (hash) {
      setTimeout(() => {
        const target = document.querySelector(hash);
        if (target) {
          // Open accordion first
          const grid = target.querySelector('.trainer-section-grid');
          const chevron = target.querySelector('.section-chevron');
          if (grid && grid.style.display === 'none') {
            grid.style.display = '';
            grid.classList.add('open');
            if (chevron) chevron.style.transform = 'rotate(180deg)';
          }
          const top = target.getBoundingClientRect().top + window.scrollY - 84;
          window.scrollTo({ top, behavior: 'smooth' });
          target.classList.add('section-highlight');
          setTimeout(() => target.classList.remove('section-highlight'), 2200);
        }
      }, 450);
    }

  } catch (err) {
    console.error(err);
    container.innerHTML = '';
    if (errorMsg) {
      const isNet = err instanceof TypeError && err.message.includes('fetch');
      errorMsg.innerHTML = isNet
        ? '⚠️ Could not connect to the server. Make sure the backend is running on <strong>localhost:7132</strong>.'
        : `⚠️ Failed to load trainers: ${err.message}`;
    }
  }
}

// ── Render sections ───────────────────────────────────────────────────────────
function renderSections(activities, trainers, filtered = null) {
  const container = document.getElementById('activities-container');
  container.innerHTML = '';

  const palette = ['#e85d2f','#2ec4b6','#d4a843','#7c5cfc','#e53e3e','#00a896'];
  const workList = filtered ?? trainers;

  const sections = [];
  activities.forEach(act => {
    const actId   = act.Id   || act.id;
    const actName = act.Name || act.name || '';
    const actTrainers = workList.filter(t => {
      const ids = t.ActivityIds || t.activityIds || [];
      return ids.includes(actId) || ids.includes(Number(actId));
    });
    if (actTrainers.length > 0) sections.push({ actId, actName, trainers: actTrainers });
  });

  const unassigned = workList.filter(t => (t.ActivityIds || t.activityIds || []).length === 0);

  let html = '';

  sections.forEach((sec, si) => {
    const isOpen = si === 0; // first section open by default
    html += buildSection(
      `activity-${sec.actId}`,
      sec.actName,
      'bi-lightning-charge-fill',
      'Activity',
      sec.trainers,
      palette,
      isOpen
    );
  });

  if (unassigned.length > 0) {
    html += buildSection(
      'activity-unassigned',
      'Other Trainers',
      'bi-person-fill',
      'General',
      unassigned,
      palette,
      sections.length === 0,
      '#64748b',
      'rgba(100,116,139,.12)'
    );
  }

  if (!html) {
    container.innerHTML = `<p class="text-center text-muted py-5">No trainers found.</p>`;
    return;
  }

  container.innerHTML = html;
}

// ── Build one accordion section ───────────────────────────────────────────────
function buildSection(id, title, icon, label, trainers, palette, open = false,
                      iconColor = 'var(--clubly-accent,#e85d2f)',
                      iconBg    = 'rgba(232,93,47,.12)') {
  const gridId = `grid-${id}`;
  return `
  <div class="trainer-section ${open ? 'is-open' : ''}" id="${id}" data-section-name="${title.toLowerCase()}">
    <button class="trainer-section-header" onclick="toggleSection('${id}')" aria-expanded="${open}">
      <div class="trainer-section-icon" style="background:${iconBg};color:${iconColor};">
        <i class="bi ${icon}"></i>
      </div>
      <div class="section-titles">
        <div class="trainer-section-label">${label}</div>
        <div class="trainer-section-title">${title}</div>
      </div>
      <div class="trainer-section-count">
        <i class="bi bi-person-fill me-1"></i>${trainers.length} Trainer${trainers.length > 1 ? 's' : ''}
      </div>
      <i class="bi bi-chevron-down section-chevron" style="transition:transform .3s;${open ? 'transform:rotate(180deg)' : ''}"></i>
    </button>
    <div class="trainer-section-grid" id="${gridId}" style="${open ? '' : 'display:none'}">
      <div class="row g-4 pt-3 pb-2">
        ${trainers.map((t, i) => trainerCardHtml(t, i, palette)).join('')}
      </div>
    </div>
  </div>`;
}

// ── Toggle accordion ──────────────────────────────────────────────────────────
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
    setTimeout(() => { grid.style.display = 'none'; grid.style.maxHeight = ''; grid.style.opacity = ''; }, 360);
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

// ── Trainer card HTML ─────────────────────────────────────────────────────────
function trainerCardHtml(t, i, palette) {
  const fullName  = t.FullName || t.fullName || '—';
  const activities = t.Activities || t.activities || '';
  const exp       = t.YearsOfExperience ?? t.yearsOfExperience ?? null;
  const email     = t.Email  || t.email  || null;
  const phone     = t.Phone  || t.phone  || null;
  const imgSrc    = (t.ImageUrl || t.imageUrl) ? 'https://localhost:7132' + (t.ImageUrl || t.imageUrl) : null;
const ratingRaw = t.Rating ?? t.rating ?? null;
const rating = ratingRaw !== null && ratingRaw !== '' ? parseFloat(ratingRaw) : null;
  const color     = palette[i % palette.length];
  const initials  = fullName.split(' ').map(w => w[0]).slice(0, 2).join('').toUpperCase();
  const id        = t.Id || t.id || i;

  return `
  <div class="col-sm-6 col-md-4 col-lg-3 trainer-item"
       data-name="${fullName.toLowerCase()}"
       data-activity="${activities.toLowerCase()}"
       data-exp="${exp ?? 0}">
    <div class="trainer-card-wrap card-cl">
      <div class="trainer-img-wrap">
        ${imgSrc
          ? `<img src="${imgSrc}" alt="${fullName}" class="trainer-photo"
                  onerror="this.style.display='none';this.nextElementSibling.style.display='flex'">
             <div class="trainer-ava" style="background:linear-gradient(135deg,${color},${color}bb);display:none;">${initials}</div>`
          : `<div class="trainer-ava" style="background:linear-gradient(135deg,${color},${color}bb);">${initials}</div>`}
        ${activities ? `<div class="trainer-specialty-badge">${activities}</div>` : ''}
      </div>
      <div class="trainer-card-body">
        <div class="trainer-name">${fullName}</div>
        ${rating !== null ? `<div class="trainer-rating">${renderStars(rating)}<span class="rating-num">${rating.toFixed(1)}</span></div>` : ''}
        ${exp !== null ? `<div class="trainer-info-row"><i class="bi bi-clock-history"></i><span>${exp} years experience</span></div>` : ''}
        ${email ? `<div class="trainer-info-row"><i class="bi bi-envelope-fill"></i><span>${email}</span></div>` : ''}
        ${phone ? `<div class="trainer-info-row"><i class="bi bi-telephone-fill"></i><span>${phone}</span></div>` : ''}
        <button class="btn-book-trainer" onclick="handleBookTrainer('${id}')">
          <i class="bi bi-calendar-plus me-1"></i>Book Session
        </button>
      </div>
    </div>
  </div>`;
}

// ── Bind all filters ──────────────────────────────────────────────────────────
function bindFilters() {
  document.getElementById('searchInput')?.addEventListener('input',  applyTrainerFilters);
  document.getElementById('activityFilter')?.addEventListener('change', applyTrainerFilters);
  document.getElementById('expFilter')?.addEventListener('change',   applyTrainerFilters);
}

function applyTrainerFilters() {
  const search  = (document.getElementById('searchInput')?.value   || '').toLowerCase().trim();
  const actVal  = (document.getElementById('activityFilter')?.value || '').toLowerCase().trim();
  const expVal  =  document.getElementById('expFilter')?.value      || '';

  document.querySelectorAll('.trainer-section').forEach(section => {
    const items = section.querySelectorAll('.trainer-item');
    let hits = 0;

    items.forEach(item => {
      const name    = item.dataset.name     || '';
      const act     = item.dataset.activity || '';
      const expNum  = parseInt(item.dataset.exp || '0');

      let show = (!search || name.includes(search) || act.includes(search))
              && (!actVal || act.includes(actVal));

      if (expVal === 'junior') show = show && expNum >= 1 && expNum <= 3;
      if (expVal === 'mid')    show = show && expNum >= 4 && expNum <= 7;
      if (expVal === 'senior') show = show && expNum >= 8;

      item.style.display = show ? '' : 'none';
      if (show) hits++;
    });

    // Show/hide section
    section.style.display = hits > 0 ? '' : 'none';

    // Auto-open section if has results and filters active
    if (hits > 0 && (search || actVal || expVal)) {
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

function clearTrainerFilters() {
  ['searchInput','activityFilter','expFilter'].forEach(id => {
    const el = document.getElementById(id);
    if (el) el.value = '';
  });
  applyTrainerFilters();
}

// ── Book ──────────────────────────────────────────────────────────────────────
async function handleBookTrainer(id) {
  const trainer = (window._trainersData || []).find(t => (t.Id || t.id) == id);
  const name    = trainer ? (trainer.FullName || trainer.fullName || 'Trainer') : 'Trainer';

  let trainerGroups = [];
  try {
    const res = await fetch('https://localhost:7132/api/ActivityGroups');
    const all = res.ok ? await res.json() : [];
    trainerGroups = all.filter(g =>
      (g.trainerId || g.TrainerId) == id &&
      (g.status || g.Status || '').toLowerCase() === 'active'
    );
  } catch(e) {}

  openBookingModal('trainer', id, name, 0, [], trainerGroups);
}

function renderStars(r) {
  return [1,2,3,4,5].map(i =>
    r>=i ? '<i class="bi bi-star-fill"></i>' : r>=i-.5 ? '<i class="bi bi-star-half"></i>' : '<i class="bi bi-star"></i>'
  ).join('');
}
