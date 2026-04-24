// ── Booking Modal ─────────────────────────────────────────────────────────────
// Full booking forms: Activity / Facility / Trainer → Payment screen

(function () {

  /* ══════════════════════════════════════════════════════════════════════════
     STYLES
  ══════════════════════════════════════════════════════════════════════════ */
  const STYLES = `
    #bpOverlay {
      display:none;position:fixed;inset:0;z-index:99998;
      background:rgba(10,22,40,.72);backdrop-filter:blur(6px);
      align-items:center;justify-content:center;padding:16px;
    }
    #bpBox {
      background:#fff;border-radius:20px;width:100%;max-width:520px;
      max-height:92vh;overflow-y:auto;box-shadow:0 24px 80px rgba(0,0,0,.3);
      font-family:'Cairo',sans-serif;animation:bpIn .28s cubic-bezier(.22,.68,0,1.2) both;
    }
    #bpBox::-webkit-scrollbar{width:5px;}
    #bpBox::-webkit-scrollbar-thumb{background:#dde;border-radius:99px;}
    @keyframes bpIn{from{opacity:0;transform:scale(.93) translateY(20px)}to{opacity:1;transform:none}}
    .bp-header {
      background:linear-gradient(135deg,#0d1b2a,#1a2f4a);
      border-radius:20px 20px 0 0;padding:20px 24px 16px;
      display:flex;align-items:center;gap:14px;position:relative;
    }
    .bp-header-icon {
      width:46px;height:46px;border-radius:13px;flex-shrink:0;
      display:flex;align-items:center;justify-content:center;font-size:22px;
    }
    .bp-header-label{color:rgba(255,255,255,.5);font-size:11px;font-weight:700;
      text-transform:uppercase;letter-spacing:.7px;}
    .bp-header-title{color:#fff;font-size:1.05rem;font-weight:900;}
    .bp-close {
      position:absolute;top:14px;right:14px;background:rgba(255,255,255,.12);
      border:none;width:32px;height:32px;border-radius:50%;cursor:pointer;
      color:#fff;font-size:16px;display:flex;align-items:center;justify-content:center;
      transition:background .2s;
    }
    .bp-close:hover{background:rgba(255,255,255,.22);}
    .bp-body{padding:24px;}
    .bp-section-title {
      font-size:1rem;font-weight:800;color:#1a2332;text-align:center;
      margin-bottom:20px;padding-bottom:12px;border-bottom:2px solid #f0f4f8;
    }
    .bp-grid{display:grid;grid-template-columns:1fr 1fr;gap:14px;margin-bottom:14px;}
    .bp-grid-1{grid-template-columns:1fr;}
    @media(max-width:480px){.bp-grid{grid-template-columns:1fr;}}
    .bp-field{display:flex;flex-direction:column;gap:5px;}
    .bp-label{font-size:11.5px;font-weight:700;color:#64748b;
      text-transform:uppercase;letter-spacing:.5px;}
    .bp-input, .bp-select {
      width:100%;border:1.5px solid #e2e8f0;border-radius:11px;
      padding:11px 14px;font-family:'Cairo',sans-serif;font-size:.9rem;
      color:#1a2332;outline:none;background:#f8fafc;
      transition:border-color .2s,background .2s;
    }
    .bp-input:focus,.bp-select:focus{border-color:#1a2f4a;background:#fff;}
    .bp-select-wrap{position:relative;}
    .bp-select-wrap select{appearance:none;-webkit-appearance:none;padding-right:36px;}
    .bp-select-wrap::after{
      content:'';position:absolute;right:14px;top:50%;transform:translateY(-50%);
      border:5px solid transparent;border-top:6px solid #94a3b8;pointer-events:none;
    }
    .bp-number-wrap{position:relative;}
    .bp-number-arrows {
      position:absolute;right:10px;top:50%;transform:translateY(-50%);
      display:flex;flex-direction:column;gap:1px;
    }
    .bp-arr{background:none;border:none;cursor:pointer;color:#94a3b8;
      font-size:10px;line-height:1;padding:2px 3px;}
    .bp-arr:hover{color:#1a2f4a;}
    .bp-error{display:none;background:#fff8f8;border:1px solid #fecaca;
      border-radius:10px;padding:10px 14px;color:#e53e3e;font-size:.83rem;
      font-weight:600;margin-bottom:14px;}
    .bp-btn-row{display:flex;gap:10px;margin-top:20px;}
    .bp-btn-cancel{
      flex:1;background:#f1f5f9;border:none;border-radius:50px;padding:13px;
      font-family:'Cairo',sans-serif;font-weight:700;font-size:.9rem;
      color:#64748b;cursor:pointer;transition:background .2s;
    }
    .bp-btn-cancel:hover{background:#e2e8f0;}
    .bp-btn-primary{
      flex:2;background:linear-gradient(135deg,#1a2f4a,#0d1b2a);color:#fff;
      border:none;border-radius:50px;padding:13px;font-family:'Cairo',sans-serif;
      font-weight:900;font-size:.9rem;cursor:pointer;
      box-shadow:0 4px 18px rgba(13,27,42,.25);transition:all .2s;
    }
    .bp-btn-primary:hover{transform:translateY(-1px);box-shadow:0 6px 22px rgba(13,27,42,.35);}
    .bp-btn-primary:disabled{opacity:.6;cursor:not-allowed;transform:none;}
    .bp-pay-info{background:#f8fafc;border-radius:14px;padding:16px 18px;margin-bottom:18px;}
    .bp-pay-info-title{font-size:.85rem;font-weight:800;color:#1a2332;margin-bottom:10px;}
    .bp-pay-row{display:flex;justify-content:space-between;align-items:center;
      padding:5px 0;border-bottom:1px solid #e8edf2;font-size:.85rem;}
    .bp-pay-row:last-child{border-bottom:none;}
    .bp-pay-row span:first-child{color:#64748b;font-weight:600;}
    .bp-pay-row span:last-child{color:#1a2332;font-weight:700;}
    .bp-total-row{display:flex;justify-content:space-between;align-items:center;
      padding:10px 0 0;font-size:1rem;font-weight:900;}
    .bp-total-price{color:#e85d2f;font-size:1.15rem;}
    .bp-tabs{display:flex;background:#f0f4f8;border-radius:10px;padding:3px;
      gap:3px;margin-bottom:16px;}
    .bp-tab{flex:1;padding:9px;text-align:center;border:none;border-radius:8px;
      font-family:'Cairo',sans-serif;font-weight:700;font-size:.85rem;
      cursor:pointer;transition:all .2s;background:none;color:#64748b;}
    .bp-tab.active{background:#1a2f4a;color:#fff;box-shadow:0 2px 10px rgba(13,27,42,.2);}
    .bp-upload{border:2px dashed #e2e8f0;border-radius:11px;padding:16px;
      text-align:center;cursor:pointer;transition:all .2s;background:#f8fafc;display:block;}
    .bp-upload:hover{border-color:#1a2f4a;background:#f0f4f8;}
    .bp-upload input{display:none;}
    .bp-upload-icon{font-size:1.4rem;margin-bottom:4px;}
    .bp-upload-text{font-size:.85rem;color:#64748b;font-weight:600;}
    .bp-upload.has-file{border-color:#00a896;background:#f0fdf9;}
    .bp-info-box{border-radius:11px;padding:12px 14px;margin-bottom:14px;
      font-size:.83rem;font-weight:600;line-height:2;}
  `;

  /* ══════════════════════════════════════════════════════════════════════════
     INJECT
  ══════════════════════════════════════════════════════════════════════════ */
  function ensureModal() {
    if (document.getElementById('bpOverlay')) return;
    const s = document.createElement('style');
    s.textContent = STYLES;
    document.head.appendChild(s);

    document.body.insertAdjacentHTML('beforeend', `
      <div id="bpOverlay">
        <div id="bpBox">
          <div class="bp-header">
            <div class="bp-header-icon" id="bpHdrIcon">📅</div>
            <div>
              <div class="bp-header-label" id="bpHdrLabel">Booking</div>
              <div class="bp-header-title"  id="bpHdrTitle">Book Now</div>
            </div>
            <button class="bp-close" onclick="closeBookingModal()">✕</button>
          </div>
          <div class="bp-body" id="bpBody"></div>
        </div>
      </div>`);

    document.getElementById('bpOverlay').addEventListener('click', e => {
      if (e.target.id === 'bpOverlay') closeBookingModal();
    });
    document.addEventListener('keydown', e => {
      if (e.key === 'Escape') closeBookingModal();
    });
  }

  /* ══════════════════════════════════════════════════════════════════════════
     STATE
  ══════════════════════════════════════════════════════════════════════════ */
  let _ctx = null;
  let _formData = {};

  /* ══════════════════════════════════════════════════════════════════════════
     PUBLIC API
  ══════════════════════════════════════════════════════════════════════════ */
window.openBookingModal = function (type, id, name, slotPrice = 0, slots = [], trainerGroups = [], scheduleId = 0) {
  ensureModal();
  _ctx = { type, id, name, slotPrice, slots, trainerGroups, scheduleId };
    _formData = {};
    setHeader(type, name);
    renderStep1();
    const ov = document.getElementById('bpOverlay');
    ov.style.display = 'flex';
    document.body.style.overflow = 'hidden';
  };
window.closeBookingModal = function () {
    const ov = document.getElementById('bpOverlay');
    if (ov) ov.style.display = 'none';
    document.body.style.overflow = '';
    _ctx = null; _formData = {};
    window._bpReceiptBase64 = ""; // تصفير الصورة عند الإغلاق
  };

  function setHeader(type, name) {
    const cfg = {
      activity: { icon:'🏃', label:'Activity Booking', bg:'rgba(232,93,47,.2)' },
      facility: { icon:'🏟️',  label:'Facility Booking', bg:'rgba(46,196,182,.2)' },
      trainer:  { icon:'🏆', label:'Trainer Session',   bg:'rgba(124,92,252,.2)' },
    };
    const c = cfg[type] || cfg.activity;
    document.getElementById('bpHdrIcon').textContent      = c.icon;
    document.getElementById('bpHdrIcon').style.background = c.bg;
    document.getElementById('bpHdrLabel').textContent     = c.label;
    document.getElementById('bpHdrTitle').textContent     = name || 'Book Now';
  }

  /* ══════════════════════════════════════════════════════════════════════════
     STEP 1 — BOOKING FORM
  ══════════════════════════════════════════════════════════════════════════ */
  function renderStep1() {
    const body = document.getElementById('bpBody');
    const formHtml = _ctx.type === 'activity' ? activityForm()
                   : _ctx.type === 'facility' ? facilityForm()
                   : trainerForm();

    body.innerHTML = `
      <div class="bp-section-title">Booking Details</div>
      <div id="bpErr" class="bp-error"></div>
      ${formHtml}
      <div class="bp-btn-row">
        <button class="bp-btn-cancel" onclick="closeBookingModal()">Cancel</button>
        <button class="bp-btn-primary" onclick="bpStep1Next()">Proceed to payment</button>
      </div>`;

    // Restore values if user hit "Back"
    ['bpDate','bpTime','bpDuration','bpParticipants','bpTrainer','bpGroup','bpSessionType']
      .forEach(id => { if (_formData[id.replace('bp','').toLowerCase()]) sv(id, _formData[id.replace('bp','').toLowerCase()]); });
    if (_formData.date)         sv('bpDate', _formData.date);
    if (_formData.time)         sv('bpTime', _formData.time);
    if (_formData.duration)     sv('bpDuration', _formData.duration);
    if (_formData.participants) sv('bpParticipants', _formData.participants);
    if (_formData.trainer)      sv('bpTrainer', _formData.trainer);
    if (_formData.group)        sv('bpGroup', _formData.group);
    if (_formData.sessionType)  sv('bpSessionType', _formData.sessionType);
  }

  /* ── Activity ──────────────────────────────────────────────────────────── */
function activityForm() {
  return `
  <div class="bp-grid">
    <div class="bp-field">
      <label class="bp-label">Activity</label>
      <input class="bp-input" value="${esc(_ctx.name)}" readonly style="color:#94a3b8;">
    </div>
    <div class="bp-field">
      <label class="bp-label">Number of Participants</label>
      <div class="bp-number-wrap">
        <input type="number" class="bp-input" id="bpParticipants" value="1" min="1" max="200" style="padding-right:36px;">
        <div class="bp-number-arrows">
          <button class="bp-arr" onclick="bpAdj('bpParticipants',1)">▲</button>
          <button class="bp-arr" onclick="bpAdj('bpParticipants',-1)">▼</button>
        </div>
      </div>
    </div>
  </div>
  <div class="bp-grid bp-grid-1">
    <div class="bp-field">
      <label class="bp-label">Select Date</label>
      <input type="date" class="bp-input" id="bpDate" min="${todayStr()}">
    </div>
  </div>
  <input type="hidden" id="bpTime" value="">
  <input type="hidden" id="bpEndTime" value="">
  <div class="bp-grid bp-grid-1" id="bpEndDateWrap" style="display:none;">
    <div class="bp-field">
      <label class="bp-label">End Date</label>
      <input type="date" class="bp-input" id="bpEndDate" readonly
        style="background:var(--bg,#f0f4f8);color:var(--muted,#64748b);cursor:not-allowed;pointer-events:none;">
    </div>
  </div>`;

}
  

  /* ── Facility ──────────────────────────────────────────────────────────── */
  function facilityForm() {
    return `
    <div class="bp-grid">
      <div class="bp-field">
        <label class="bp-label">Facility</label>
        <input class="bp-input" value="${esc(_ctx.name)}" readonly style="color:#94a3b8;">
      </div>
      <div class="bp-field">
        <label class="bp-label">Number of Participants</label>
        <input type="number" class="bp-input" id="bpParticipants"
               placeholder="Number of participants" min="1">
      </div>
    </div>
    <div class="bp-grid">
      <div class="bp-field">
        <label class="bp-label">Select Date</label>
        <input type="date" class="bp-input" id="bpDate" min="${todayStr()}">
      </div>
      <div class="bp-field">
        <label class="bp-label">Select Time</label>
        <input type="time" class="bp-input" id="bpTime">
      </div>
    </div>
    <div class="bp-grid bp-grid-1">
      <div class="bp-field">
        <label class="bp-label">End Time</label>
        <input type="time" class="bp-input" id="bpEndTime"
               style="color:#94a3b8;cursor:not-allowed;" readonly>
      </div>
    </div>`;
  }

  /* ── Trainer ───────────────────────────────────────────────────────────── */
function trainerForm() {
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  const groupOptions = (_ctx.trainerGroups || []).map(g => {
    const slots = g.timeSlots || g.TimeSlots || [];
    const slotsData = JSON.stringify(slots.map(s => ({
      day:   s.day || s.Day || '',
      start: (s.startTime || '').substring(0, 5),
      end:   (s.endTime   || '').substring(0, 5)
    })));
    const isExpired = g._isExpired || false;

    return `<option value="${g.id}"
      data-slots='${slotsData}'
      data-duration="${g.durationDays || ''}"
      data-price="${g.price || 0}"
      data-name="${esc(g.name || '')}"
      data-expired="${isExpired}"
      ${isExpired ? 'disabled style="color:#94a3b8;"' : ''}>
      ${isExpired ? '🔒 ' : ''}${g.name}${g.price ? ' — ' + g.price + ' EGP' : ''}${isExpired ? ' (Expired)' : ''}
    </option>`;
  }).join('');

  return `
  <div class="bp-grid">
    <div class="bp-field">
      <label class="bp-label">Trainer</label>
      <input class="bp-input" value="${esc(_ctx.name)}" readonly style="color:#94a3b8;">
    </div>
    <div class="bp-field">
      <label class="bp-label">Number of Participants</label>
      <div class="bp-number-wrap">
        <input type="number" class="bp-input" id="bpParticipants" value="1" min="1" max="20" style="padding-right:36px;">
        <div class="bp-number-arrows">
          <button class="bp-arr" onclick="bpAdj('bpParticipants',1)">▲</button>
          <button class="bp-arr" onclick="bpAdj('bpParticipants',-1)">▼</button>
        </div>
      </div>
    </div>
  </div>
  <div class="bp-grid bp-grid-1">
    <div class="bp-field">
      <label class="bp-label">Select Group</label>
      <div class="bp-select-wrap">
        <select class="bp-select" id="bpTrainerGroup" onchange="bpTrainerGroupChange()">
          <option value="">Select a group...</option>
          ${groupOptions}
        </select>
      </div>
    </div>
  </div>
  <div id="bpSlotsDisplay" style="display:none;margin-bottom:12px;">
    <div style="font-size:.75rem;font-weight:700;color:#64748b;margin-bottom:6px;text-transform:uppercase;letter-spacing:.5px;">Available Time Slots</div>
    <div id="bpSlotsInner"></div>
  </div>
  <div class="bp-grid">
    <div class="bp-field">
      <label class="bp-label">Start Date</label>
      <input type="date" class="bp-input" id="bpDate" value="${todayStr()}" readonly
        style="background:#f0f4f8;color:#64748b;cursor:not-allowed;pointer-events:none;">
      <div style="font-size:.68rem;font-weight:700;color:#e85d2f;margin-top:3px;">
        <i class="bi bi-lock-fill" style="font-size:.6rem"></i> Today's date
      </div>
    </div>
    <div class="bp-field">
      <label class="bp-label">End Date</label>
      <input type="date" class="bp-input" id="bpEndDate" readonly
        style="background:#f0f4f8;color:#64748b;cursor:not-allowed;pointer-events:none;">
      <div id="bpEndDateNote" style="font-size:.68rem;font-weight:700;color:#e85d2f;margin-top:3px;"></div>
    </div>
  </div>
 <input type="hidden" id="bpTime" value="">
  <input type="hidden" id="bpEndTime" value="">
  <input type="hidden" id="bpDuration" value="">`;
}
window.bpTrainerGroupChange = function() {
  const sel = document.getElementById('bpTrainerGroup');
  const opt = sel?.selectedOptions[0];
  if (!opt || !opt.value) return;

  // ✅ تحقق لو expired
  const isExpired = opt.dataset.expired === 'true';
  if (isExpired) {
    // إعادة تعطيل الـ Confirm
    const confirmBtn = document.querySelector('.bp-btn-primary');
    if (confirmBtn) {
      confirmBtn.style.opacity      = '.4';
      confirmBtn.style.pointerEvents = 'none';
    }
    // إظهار رسالة خطأ
    const errEl = document.getElementById('bpErr');
    if (errEl) {
      errEl.innerHTML = `<i class="bi bi-x-circle-fill me-1"></i>
        This group has already ended — booking is no longer available.`;
      errEl.style.display = 'block';
    }
    // إخفاء الـ slots display
    const slotsDisplay = document.getElementById('bpSlotsDisplay');
    if (slotsDisplay) slotsDisplay.style.display = 'none';
    return; // ⛔ وقف هنا
  }

  // ✅ لو مش expired امسح الـ error وكمّل عادي
  const errEl = document.getElementById('bpErr');
  if (errEl) errEl.style.display = 'none';

  const slots    = JSON.parse(opt.dataset.slots    || '[]');
  const duration = opt.dataset.duration || '';
  const price    = opt.dataset.price    || '0';
  const gname    = opt.dataset.name     || opt.text.split('—')[0].trim();

  // Start/End Time من أول slot
  const timeEl    = document.getElementById('bpTime');
  const endTimeEl = document.getElementById('bpEndTime');
  if (slots.length) {
    if (timeEl)    timeEl.value    = slots[0].start;
    if (endTimeEl) endTimeEl.value = slots[0].end;
  }

  // عرض كل الـ slots
  const slotsDisplay = document.getElementById('bpSlotsDisplay');
  const slotsInner   = document.getElementById('bpSlotsInner');
  if (slots.length && slotsDisplay && slotsInner) {
    slotsInner.innerHTML = slots.map(s => `
      <span style="display:inline-flex;align-items:center;gap:5px;font-size:.78rem;font-weight:700;
        padding:5px 12px;border-radius:20px;background:rgba(232,93,47,.08);
        color:#e85d2f;border:1px solid rgba(232,93,47,.2);margin:3px;">
        ${s.day ? `<span style="background:#64748b;color:#fff;padding:2px 7px;border-radius:10px;font-size:.7rem;">${s.day}</span>` : ''}
        <i class="bi bi-clock" style="font-size:.65rem"></i> ${s.start} → ${s.end}
      </span>`).join('');
    slotsDisplay.style.display = '';
  }

  // End Date من duration
  const endDateEl   = document.getElementById('bpEndDate');
  const endDateNote = document.getElementById('bpEndDateNote');
  if (endDateEl && duration) {
    const d = new Date();
    d.setDate(d.getDate() + parseInt(duration));
    endDateEl.value = d.toISOString().split('T')[0];
    if (endDateNote) endDateNote.innerHTML =
      `<i class="bi bi-lock-fill" style="font-size:.6rem"></i> Calculated from duration (${duration} days)`;
  }

  _ctx.slotPrice  = parseFloat(price) || 0;
  _ctx.groupName  = gname;
  document.getElementById('bpDuration').value = duration;
};

  /* ── Validate & proceed ────────────────────────────────────────────────── */
  window.bpStep1Next = function () {
    const date = v('bpDate');
    const time = v('bpTime');
   if (!date) { showErr('Please select a date.'); return; }
if (_ctx.type === 'trainer' && !v('bpTrainerGroup')) { showErr('Please select a group.'); return; }
if (_ctx.type !== 'activity' && _ctx.type !== 'trainer' && !time) { showErr('Please select a time.'); return; }
const isFacility = _ctx.type === 'facility';
const isActivity = _ctx.type === 'activity';
const dur     = (isFacility || isActivity) ? '' : v('bpDuration');
const endTime = isFacility ? v('bpEndTime') : '';

if (!isFacility && !isActivity && !dur) { showErr('Please select a duration.'); return; }

    const base = { activity:150, facility:200, trainer:300 };
    // For facility, calculate mins from start→end; fallback 60
    let mins = 60;
    if (isFacility && time && endTime) {
      const [sh, sm] = time.split(':').map(Number);
      const [eh, em] = endTime.split(':').map(Number);
      mins = Math.max(15, (eh * 60 + em) - (sh * 60 + sm));
    } else if (!isFacility) {
      mins = parseInt(dur) || 60;
    }
    const pax = parseInt(v('bpParticipants') || '1') || 1;

    _formData = {
  type: _ctx.type, id: _ctx.id, name: _ctx.name,
  date, time, endTime, duration: isFacility ? String(mins) : dur,
  participants: String(pax),
  trainer:     v('bpTrainer')     || '',
  group:       v('bpGroup')       || '',
  sessionType: v('bpSessionType') || '',
  groupName:   _ctx.groupName     || '',
  endDate:     document.getElementById('bpEndDate')?.value || '',date: document.getElementById('bpDate')?.value || date,
  slots:       _ctx.slots         || [],
  price: (_ctx.slotPrice || 0) > 0
    ? (_ctx.slotPrice || 0) * pax
    : (base[_ctx.type] || 150) * (mins / 60) * pax,
};
    renderPayment();
  };

  /* ══════════════════════════════════════════════════════════════════════════
     STEP 2 — PAYMENT
  ══════════════════════════════════════════════════════════════════════════ */
  function renderPayment() {
    document.getElementById('bpHdrIcon').textContent      = '💳';
    document.getElementById('bpHdrIcon').style.background = 'rgba(232,93,47,.18)';
    document.getElementById('bpHdrLabel').textContent     = 'Payment';
    document.getElementById('bpHdrTitle').textContent     = 'Payment Details';

    const typeLabel = { activity:'Activity', facility:'Facility', trainer:'Trainer' };
    const durLabel  = fmtDur(_formData.duration);

  const endDateEl = document.getElementById('bpEndDate');
const endDateVal = endDateEl ? endDateEl.value : '';

// جيب الـ slots من _ctx عشان تكون محدثة
const trainerSlots = window.bpTrainerGroupChange
  ? (JSON.parse(document.getElementById('bpTrainerGroup')
      ?.selectedOptions[0]?.dataset?.slots || '[]'))
  : [];

const rows = [
  [typeLabel[_formData.type]||'Item', _formData.name],
  ...(_formData.groupName ? [['Group', _formData.groupName]] : []),
  ['Date', fmtDate(_formData.date)],
  ...(_formData.endDate ? [['End Date', fmtDate(_formData.endDate)]] : []),
  ...(_formData.type === 'facility'
    ? [['Time', fmtTime(_formData.time)], ['End Time', fmtTime(_formData.endTime)]]
    : []),
  ...(_formData.type === 'trainer' && trainerSlots.length
    ? trainerSlots.map(s => [s.day ? `Slot (${s.day})` : 'Slot', `${s.start} → ${s.end}`])
    : []),
  ['Participants', _formData.participants],
  ...(_ctx.slotPrice > 0 ? [['Price per person', `${_ctx.slotPrice} EGP`]] : []),
];
    if (_formData.trainer && _formData.trainer !== '')
      rows.push(['Trainer', trainerName(_formData.trainer)]);
    if (_formData.sessionType)
      rows.push(['Session', cap(_formData.sessionType)]);

    const rowsHtml = rows.map(([l, val]) =>
      `<div class="bp-pay-row"><span>${l}:</span><span>${val||'—'}</span></div>`
    ).join('');

    document.getElementById('bpBody').innerHTML = `
      <div class="bp-section-title" style="color:#e85d2f;">Payment Details</div>

      <div class="bp-pay-info">
        <div class="bp-pay-info-title">Booking Information</div>
        ${rowsHtml}
        <div class="bp-total-row">
          <span>Total Price :</span>
          <span class="bp-total-price">${_formData.price.toFixed(0)} EGP</span>
        </div>
      </div>

      <div class="bp-tabs">
        <button class="bp-tab active" id="tabI" onclick="bpTab('I')">InstaPay</button>
        <button class="bp-tab"        id="tabW" onclick="bpTab('W')">E-Wallets</button>
      </div>

      <div id="panelI">
        <div class="bp-info-box" style="background:#f0f9ff;color:#0369a1;">
          📱 Receiver InstaPay Number: <strong>01012345678</strong><br>
          🏦 Receiver Account: <strong>clubly@instapay</strong>
        </div>
        ${payFields('I', _formData.price)}
      </div>

      <div id="panelW" style="display:none;">
        <div class="bp-info-box" style="background:#f0fdf4;color:#166534;">
          📱 Vodafone Cash: <strong>01012345678</strong><br>
          📱 Orange Cash: <strong>01112345678</strong><br>
          📱 Etisalat Cash: <strong>01512345678</strong>
        </div>
        ${payFields('W', _formData.price)}
      </div>

      <div id="bpErr" class="bp-error" style="margin-top:14px;"></div>

      <div class="bp-btn-row">
        <button class="bp-btn-cancel" onclick="bpBack()">← Back</button>
        <button class="bp-btn-primary" id="bpPayBtn" onclick="bpPay()">
          Complete Payment
        </button>
      </div>`;
  }

  function payFields(suf, price) {
    return `
    <div class="bp-grid">
      <div class="bp-field">
        <label class="bp-label">Amount (EGP)</label>
        <input class="bp-input" id="bpAmt${suf}" type="number"
               value="${price.toFixed(0)}" placeholder="Amount">
      </div>
      <div class="bp-field">
        <label class="bp-label">Transaction ID</label>
        <input class="bp-input" id="bpTx${suf}" placeholder="Transaction ID">
      </div>
    </div>
    <div class="bp-field" style="margin-bottom:4px;">
<label class="bp-label">Upload Receipt <span style="color:#e53e3e">*</span></label>
      <label class="bp-upload" id="bpUL${suf}">
        <input type="file" accept="image/*,application/pdf"
               onchange="bpFile(this,'${suf}')">
        <div class="bp-upload-icon">📎</div>
        <div class="bp-upload-text" id="bpUT${suf}">Click to upload receipt</div>
      </label>
    </div>`;
  }

  window.bpTab = function (suf) {
    document.getElementById('panelI').style.display = suf === 'I' ? '' : 'none';
    document.getElementById('panelW').style.display = suf === 'W' ? '' : 'none';
    document.getElementById('tabI').classList.toggle('active', suf === 'I');
    document.getElementById('tabW').classList.toggle('active', suf === 'W');
  };

window.bpFile = function (input, suf) {
  if (input.files && input.files[0]) {
    const file = input.files[0];
    window._bpReceiptFile = file;  // ← غير ده
    document.getElementById('bpUT' + suf).textContent = '✅ ' + file.name;
    document.getElementById('bpUL' + suf).classList.add('has-file');
  

    // تحويل الصورة لـ Base64
    const reader = new FileReader();
    reader.onload = function (e) {
      window._bpReceiptBase64 = e.target.result; // ده الـ String اللي هنبعته
    };
    reader.readAsDataURL(file);
  }
};

  window.bpBack = function () {
    setHeader(_ctx.type, _ctx.name);
    renderStep1();
  };
function formatTime(t) {
  if (!t) return '00:00:00';
  const parts = t.split(':');
  return parts.length === 2 ? t + ':00' : t;
}
window.bpPay = async function () {
    const suf = document.getElementById('tabI').classList.contains('active') ? 'I' : 'W';
    if (!v('bpAmt' + suf)) { showErr('Please enter the amount.');         return; }
    if (!v('bpTx'  + suf)) { showErr('Please enter the Transaction ID.'); return; }
if (!window._bpReceiptFile) { 
  showErr('Please upload the receipt.'); 
  return; 
}
    const btn = document.getElementById('bpPayBtn');
    btn.disabled  = true;
    btn.innerHTML = '⏳ Processing…';

    try {
      const memberId = localStorage.getItem('userId');
      if (!memberId) { showErr('Please sign in first.'); btn.disabled=false; btn.innerHTML='Complete Payment'; return; }

 if (_formData.type === 'facility') {
        const facPayload = {
          facilityId:         _ctx.id,
          facilityScheduleId: _ctx.scheduleId || null,
          memberId:           parseInt(memberId),
          bookedByName:       localStorage.getItem('fullName') || 'Guest',
          bookedByEmail:      '',
          bookingDate:        _formData.date,
       startTime: formatTime(_formData.time),
endTime:   formatTime(_formData.endTime),
          participants:       parseInt(_formData.participants) || 1,
          paymentMethod:      suf === 'I' ? 'InstaPay' : 'E-Wallet',
          transactionId:      v('bpTx' + suf),
          price:              parseFloat(v('bpAmt' + suf)) || _formData.price,
          // السطر المهم اللي كان ناقص:
          receiptImageUrl:    window._bpReceiptBase64 || "" 
        };
        
   const fd2 = new FormData();
fd2.append('facilityId',         _ctx.id);
if (_ctx.scheduleId) fd2.append('facilityScheduleId', _ctx.scheduleId);
const userRole = localStorage.getItem('role') || 'guest';
if (userRole === 'member') {
  fd2.append('memberId', memberId);
} else {
  fd2.append('guestId', memberId);
}fd2.append('bookedByName',       localStorage.getItem('fullName') || 'Guest');
fd2.append('bookedByEmail', localStorage.getItem('email') || 'guest@clubly.com');
fd2.append('bookingDate',        _formData.date);
fd2.append('startTime',          formatTime(_formData.time));
fd2.append('endTime',            formatTime(_formData.endTime));
fd2.append('participants',       parseInt(_formData.participants) || 1);
fd2.append('paymentMethod',      suf === 'I' ? 'InstaPay' : 'E-Wallet');
fd2.append('transactionId',      v('bpTx' + suf));
fd2.append('price',              parseFloat(v('bpAmt' + suf)) || _formData.price);
fd2.append('receiptImage', window._bpReceiptFile);
const facRes = await fetch('https://localhost:7132/api/FacilityBookings', {
  method: 'POST',
  body: fd2
});
      if (!facRes.ok) {
  const err = await facRes.text(); // ← غير json لـ text
  console.log('ERROR:', err);      // ← هتشوف المشكلة بالظبط
  showErr(err);
  btn.disabled = false; btn.innerHTML = 'Complete Payment';
  return;
}

    // ... داخل ميثود bpPay ...
} else {
const fd = new FormData();
fd.append('activityId',      window._actPickerActivityId || 0);
fd.append('activityGroupId', _ctx.id);

// ✅ التعديل هنا
const userRole = localStorage.getItem('role') || 'guest';
if (userRole === 'member') {
  fd.append('memberId', memberId);
} else {
  fd.append('guestId', memberId);
}

fd.append('trainerId',       window._actPickerTrainerId || '');
fd.append('startDate',       _formData.date);
fd.append('participants',    parseInt(_formData.participants) || 1);
fd.append('paymentMethod',   suf === 'I' ? 'InstaPay' : 'EWallet');
fd.append('transactionId',   v('bpTx' + suf));

if (window._bpReceiptFile) fd.append('receiptImage', window._bpReceiptFile);

  const res = await fetch('https://localhost:7132/api/ActivityBookings', {
    method: 'POST',
    body: fd   // بدون Content-Type header خالص
  });
// ...
if (!res.ok) {
  const errText = await res.text();
  console.log('Booking error:', errText);
  showErr(errText || 'Booking failed. Please try again.');
          btn.disabled = false; btn.innerHTML = 'Complete Payment';
          return;
        }
      }

      // Success
      document.getElementById('bpBody').innerHTML = `
        <div style="text-align:center;padding:36px 20px;">
          <div style="font-size:3.5rem;margin-bottom:14px;">✅</div>
          <div style="font-size:1.1rem;font-weight:900;color:#1a2332;margin-bottom:8px;">
            Booking Submitted!
          </div>
          <div style="color:#64748b;font-size:.88rem;line-height:1.7;margin-bottom:24px;">
            Your booking is <strong style="color:#f59e0b;">Pending</strong> approval.<br>
            We'll confirm once payment is verified.
          </div>
          <button class="bp-btn-primary" onclick="closeBookingModal()"
                  style="width:100%;max-width:180px;margin:0 auto;display:block;">
            Close
          </button>
        </div>`;

    } catch(e) {
      showErr('Connection error. Please try again.');
      btn.disabled = false;
      btn.innerHTML = 'Complete Payment';
    }
  };

  /* ══════════════════════════════════════════════════════════════════════════
     HELPERS
  ══════════════════════════════════════════════════════════════════════════ */
  window.bpAdj = function (id, d) {
    const el = document.getElementById(id);
    if (!el) return;
    el.value = Math.min(+(el.max||9999), Math.max(+(el.min||1), (+el.value||1) + d));
  };

  function showErr(msg) {
    const el = document.getElementById('bpErr');
    if (el) { el.innerHTML = '⚠ ' + msg; el.style.display = 'block'; }
  }

  function v(id)       { const e = document.getElementById(id); return e ? e.value.trim() : ''; }
  function sv(id, val) { const e = document.getElementById(id); if (e) e.value = val; }
  function esc(s)      { return (s||'').replace(/"/g,'&quot;'); }
  function cap(s)      { return s ? s[0].toUpperCase() + s.slice(1) : ''; }
  function todayStr()  { return new Date().toISOString().split('T')[0]; }

  function fmtDate(d) {
    if (!d) return '—';
    const [y,m,dy] = d.split('-');
    return `${dy} ${['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'][+m-1]} ${y}`;
  }
  function fmtTime(t) {
    if (!t) return '—';
    const [h, mi] = t.split(':');
    const hr = +h;
    return `${hr%12||12}:${mi} ${hr<12?'AM':'PM'}`;
  }
  function fmtDur(d) {
    if (!d) return '—';
    const m = +d;
    return m >= 60 ? `${m/60} hour${m>60?'s':''}` : `${m} min`;
  }
  function trainerName(id) {
    const all = window._allTrainersData || window._trainersData || [];
    const t = all.find(t => (t.Id||t.id) == id);
    return t ? (t.FullName||t.fullName||id) : id;
  }

})();