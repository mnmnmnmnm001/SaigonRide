// Theme switching functionality
(function() {
    const THEME_KEY = 'saigonride-theme';
    const LIGHT_THEME = 'light';
    const DARK_THEME = 'dark';

    /**
     * Initialize theme on page load
     */
    function initTheme() {
        const savedTheme = localStorage.getItem(THEME_KEY) || LIGHT_THEME;
        applyTheme(savedTheme);
    }

    /**
     * Apply theme to the document
     */
    function applyTheme(theme) {
        const html = document.documentElement;
        if (theme === DARK_THEME) {
            html.setAttribute('data-theme', DARK_THEME);
            updateThemeToggleButton(DARK_THEME);
        } else {
            html.removeAttribute('data-theme');
            updateThemeToggleButton(LIGHT_THEME);
        }
        localStorage.setItem(THEME_KEY, theme);
    }

    /**
     * Update theme toggle button appearance
     */
    function updateThemeToggleButton(theme) {
        const btn = document.getElementById('themeToggleBtn');
        if (!btn) return;

        if (theme === DARK_THEME) {
            btn.innerHTML = '<i class="bi bi-sun-fill"></i> Light';
            btn.title = 'Switch to Light Theme';
        } else {
            btn.innerHTML = '<i class="bi bi-moon-fill"></i> Dark';
            btn.title = 'Switch to Dark Theme';
        }
    }

    /**
     * Toggle between light and dark themes
     */
    window.toggleTheme = function() {
        const html = document.documentElement;
        const currentTheme = html.getAttribute('data-theme') || LIGHT_THEME;
        const newTheme = currentTheme === LIGHT_THEME ? DARK_THEME : LIGHT_THEME;
        applyTheme(newTheme);
    };

    /**
     * Get current theme
     */
    window.getCurrentTheme = function() {
        return document.documentElement.getAttribute('data-theme') || LIGHT_THEME;
    };

    // Initialize theme when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initTheme);
    } else {
        initTheme();
    }
})();
