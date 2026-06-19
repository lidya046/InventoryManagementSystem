document.addEventListener('DOMContentLoaded', () => {
    const alerts = document.querySelectorAll('.app-alert');
    alerts.forEach((alertElement) => {
        window.setTimeout(() => {
            const instance = bootstrap.Alert.getOrCreateInstance(alertElement);
            instance.close();
        }, 4500);
    });
});
