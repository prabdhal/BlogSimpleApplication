let blogData = document.querySelector('div.blogData');

let dropdownContent = document.querySelectorAll('.dropdown-content');
let menuIcons = document.querySelectorAll('.menu-icon');

const categoryBadgeContainer = document.querySelector('#categoryBadgeContainer');


const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];


// create category badge on detail view dashboard
const createCategoryBadge = () => {
    let blog = JSON.parse(blogData.getAttribute("value"));

    const badgeElement = document.createElement('a');
    badgeElement.innerHTML = `<a class="badge text-decoration-none link-light ${getBlogCategoryClass(blog.category)}">${getBlogCategoryName(blog.category)}</a>`;

    categoryBadgeContainer.append(badgeElement);
}

// adds on click listener to all comment menus
menuIcons.forEach(menu => {
    menu.addEventListener('click', () => {
        openCommentDropDownMenu(menu);
    });
});

// opens the comment drop down menu
const openCommentDropDownMenu = (element) => {
    if (element.nextElementSibling.classList.contains('block')) {
        closeAllCommentDropDownMenu();
    } else {
        closeAllCommentDropDownMenu();
        element.nextElementSibling.classList.add('block');
    }
}

// closes all comment drop down menus
const closeAllCommentDropDownMenu = () => {
    dropdownContent.forEach(dropDown => {
        dropDown.classList.remove('block');
    });
}

// closes menus when clicking on window
window.addEventListener('click', (e) => {
    if (e.target.classList.contains('menu-icon') == false) {
        closeAllCommentDropDownMenu();
    }
})


// HELPER FUNCTIONS 
// maps enum int to its name
const getBlogCategoryName = (value) => {
    switch (value) {
        case 0:
            return 'HTML';
        case 1:
            return 'CSS';
        case 2:
            return 'JavaScript';
        case 3:
            return 'C#';
        case 4:
            return 'Object-Oriented Programming';
        case 5:
            return 'Web Design';
        case 6:
            return 'Tutorials';
        case 7:
            return 'Freebies';
        case 8:
            return 'Other';
        default:
            return 'All';
    }
}

// maps enum name into ints
const getBlogCategoryIdx = (value) => {
    switch (value) {
        case 'HTML':
            return 0;
        case 'CSS':
            return 1;
        case 'JavaScript':
            return 2;
        case 'C#':
            return 3;
        case 'Object-Oriented Programming':
            return 4;
        case 'Web Design':
            return 5;
        case 'Tutorials':
            return 6;
        case 'Freebies':
            return 7;
        case 'Other':
            return 8;
        default:
            return 100;
    }
}

// maps enum int to color
const getBlogCategoryClass = (value) => {
    switch (value) {
        case 0:
            return 'html';
        case 1:
            return 'css';
        case 2:
            return 'js';
        case 3:
            return 'cs';
        case 4:
            return 'oop';
        case 5:
            return 'web-design';
        case 6:
            return 'tutorials';
        case 7:
            return 'freebies';
        case 8:
            return 'other';
        default:
            return 'all';
    }
}

createCategoryBadge();