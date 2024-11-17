const logoutButton = document.getElementById("logout-btn");

logoutButton.addEventListener("click", () => {
    localStorage.clear();

    fetch('/Logout', { method: 'POST' })
        .then(() => {
            window.location.reload();
        });
});