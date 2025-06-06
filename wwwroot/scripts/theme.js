function colorModeInit() {
    const colorToggle = document.getElementById("color_mode");

    function applyTheme() {
        const isChecked = colorToggle?.checked;
        document.documentElement.classList.toggle("dark", isChecked);
        document.body.classList.toggle("dark-preview", isChecked);
        document.body.classList.toggle("white-preview", !isChecked);
    }

    if (colorToggle) {
        colorToggle.addEventListener("change", applyTheme);
        applyTheme(); // Apply theme immediately on page load
    }
}

function colorModePreview() {
    // Called externally (e.g. from Blazor) to update the theme
    const colorToggle = document.getElementById("color_mode");
    const isChecked = colorToggle?.checked;
    document.documentElement.classList.toggle("dark", isChecked);
    document.body.classList.toggle("dark-preview", isChecked);
    document.body.classList.toggle("white-preview", !isChecked);
}

document.addEventListener("DOMContentLoaded", colorModeInit);
