let postData = document.querySelector('div.blogData');
const categoryBadgeContainer = document.querySelector('#categoryBadgeContainer');
const postCategoryListContainer = document.querySelector('#blogCategoryListContainer');



// create category badge on detail view dashboard
const createCategoryBadge = () => {
    let post = JSON.parse(postData.getAttribute("value"));

    const badgeElement = document.createElement('a');
    badgeElement.innerHTML = `<a class="badge text-decoration-none link-light ${getPostCategoryClass(post.category)}">${getPostCategoryName(post.category)}</a>`;

    categoryBadgeContainer.append(badgeElement);
}

// HELPER FUNCTIONS 
// maps enum int to its name
const getPostCategoryName = (value) => {
    switch (value) {
        case 0:
            return 'Programming';
        case 1:
            return 'Game Development';
        case 2:
            return 'Web Development';
        case 3:
            return 'General Tutorials';
        case 4:
            return 'Other';
        default:
            return 'All';
    }
}

// maps enum name into ints
const getPostCategoryIdx = (value) => {
    switch (value) {
        case 'Programming':
            return 0;
        case 'Game Development':
            return 1;
        case 'Web Development':
            return 2;
        case 'General Tutorials':
            return 3;
        case 'Other':
            return 4;
        default:
            return 100;
    }
}

// maps enum int to color
const getPostCategoryClass = (value) => {
    switch (value) {
        case 0:
            return 'programming';
        case 1:
            return 'game-development';
        case 2:
            return 'web-development';
        case 3:
            return 'general-tutorials';
        case 4:
            return 'other';
        default:
            return 'all';
    }
}

createCategoryBadge();