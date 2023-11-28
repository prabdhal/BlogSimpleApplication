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
            return 'Technology';
        case 1:
            return 'Economy & Finance';
        case 2:
            return 'Health & Fitness';
        case 3:
            return 'Food';
        case 4:
            return 'Politics';
        case 5:
            return 'Travel';
        case 6:
            return 'Human Science';
        case 7:
            return 'Nature Science';
        default:
            return 'All';
    }
}

// maps enum name into ints
const getPostCategoryIdx = (value) => {
    switch (value) {
        case 'Technology':
            return 0;
        case 'Economy & Finance':
            return 1;
        case 'Health & Fitness':
            return 2;
        case 'Food':
            return 3;
        case 'Politics':
            return 4;
        case 'Travel':
            return 5;
        case 'Human Science':
            return 6;
        case 'Nature Science':
            return 7;
        default:
            return 100;
    }
}

// maps enum int to color
const getPostCategoryClass = (value) => {
    switch (value) {
        case 'technology':
            return 0;
        case 'economy-finance':
            return 1;
        case 'health-fitness':
            return 2;
        case 'food':
            return 3;
        case 'politics':
            return 4;
        case 'travel':
            return 5;
        case 'human-science':
            return 6;
        case 'nature-science':
            return 7;
        default:
            return 'all';
    }
}

createCategoryBadge();