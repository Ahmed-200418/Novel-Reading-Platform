// Antigravity UI & Image Fallback System for Novel Reading Platform

/**
 * Universal image error handler to generate an elegant book cover SVG placeholder
 * when an external image URL fails to load.
 */
function handleImageError(imgElement) {
    if (imgElement.dataset.failed) return; // prevent infinite loops
    imgElement.dataset.failed = "true";

    const title = imgElement.getAttribute('alt') || 'رواية جديدة';
    const cleanTitle = title.length > 25 ? title.substring(0, 22) + '...' : title;

    // SVG Data URI for an artistic book cover
    const svgCover = `
    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 300 400" width="100%" height="100%">
      <defs>
        <linearGradient id="coverGrad" x1="0%" y1="0%" x2="100%" y2="100%">
          <stop offset="0%" stop-color="#0f172a"/>
          <stop offset="50%" stop-color="#0f766e"/>
          <stop offset="100%" stop-color="#0d9488"/>
        </linearGradient>
        <linearGradient id="goldGrad" x1="0%" y1="0%" x2="100%" y2="0%">
          <stop offset="0%" stop-color="#f59e0b"/>
          <stop offset="100%" stop-color="#fbbf24"/>
        </linearGradient>
      </defs>
      <rect width="300" height="400" rx="12" fill="url(#coverGrad)"/>
      <rect x="15" y="15" width="270" height="370" rx="8" fill="none" stroke="rgba(255,255,255,0.15)" stroke-width="2"/>
      <circle cx="150" cy="140" r="45" fill="rgba(13, 148, 136, 0.3)" stroke="url(#goldGrad)" stroke-width="3"/>
      <path d="M135 125 L165 125 L165 155 L135 155 Z" fill="none" stroke="#ffffff" stroke-width="2"/>
      <path d="M150 115 L150 165 M125 140 L175 140" stroke="#f59e0b" stroke-width="2"/>
      <text x="150" y="240" fill="#ffffff" font-family="Cairo, sans-serif" font-weight="bold" font-size="20" text-anchor="middle" direction="rtl">${cleanTitle}</text>
      <text x="150" y="275" fill="#38bdf8" font-family="Cairo, sans-serif" font-size="14" text-anchor="middle" direction="rtl">منصة كِتَاب</text>
      <rect x="90" y="320" width="120" height="4" rx="2" fill="url(#goldGrad)"/>
    </svg>`;

    const encodedSvg = 'data:image/svg+xml;charset=utf-8,' + encodeURIComponent(svgCover);
    imgElement.src = encodedSvg;
}

// Automatically attach fallback handler to all image elements on page load
document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('img').forEach(img => {
        if (!img.getAttribute('onerror')) {
            img.addEventListener('error', () => handleImageError(img));
        }
    });
});
