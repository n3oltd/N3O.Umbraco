document.querySelectorAll('.toggle-btn').forEach(button => {
    button.addEventListener('click', () => {
        const content = button.nextElementSibling;
        content.style.display = content.style.display === 'block' ? 'none' : 'block';

        button.classList.toggle('active');
    });
});
