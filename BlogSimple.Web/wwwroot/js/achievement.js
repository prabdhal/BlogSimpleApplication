const achievementCompletionChecks = document.querySelectorAll('.achievement-completion-check');

const updateBadgeBackground = () => {
    achievementCompletionChecks.forEach((achievement) => {
        let completed = achievement.getAttribute('data-achievement-completed');
        if (completed == 'True') {
            achievement.nextElementSibling.classList.add('achievement-completed');
        } else {
            achievement.nextElementSibling.classList.remove('achievement-completed');
        }
    });
}

updateBadgeBackground();