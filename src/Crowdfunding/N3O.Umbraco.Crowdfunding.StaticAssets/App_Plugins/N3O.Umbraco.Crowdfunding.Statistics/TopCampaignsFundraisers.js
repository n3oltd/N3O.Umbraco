
function showContainer(containerId) {
    document.querySelectorAll('.container').forEach(container => {
        container.classList.remove('active');
    });

    document.getElementById(containerId).classList.add('active');

    document.querySelectorAll('.tab').forEach(tab => {
        tab.classList.remove('active');
    });

    document.querySelector(`.tab[onclick="showContainer('${containerId}')"]`).classList.add('active');
}
