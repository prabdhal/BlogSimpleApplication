﻿// post menu refs
let dropdownContent = document.querySelectorAll('.dropdown-content');
let menuIcons = document.querySelectorAll('.menu-icon');

// post and category containers refs
const postsDisplayContainer = document.querySelector('#blogsDisplayContainer');
const paginationNavContainer = document.querySelector('#paginationNavContainer');
const categoryBadgeContainer = document.querySelector('#categoryBadgeContainer');

// data 
let postData = document.querySelector('div.blogData');
let postsData = document.querySelectorAll('div.blogsData');

// pagination values 
const maxPostsPerPage = 10;
let currentPageNumber = 1;

// search string and category
let postSearchString = "";
let postCategoryIdx = 100;

// controller paths
let postDetailsPath = '/Post/PostDetails';
let createPostPath = '/Post/CreatePost';
let editPostPath = '/Post/EditPost';
let deletePostPath = '/Post/DeletePost';

const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];


const getPostImagePath = (img) => {
    return `data:image/jpg;base64,${img}`;
}

// opens modal to delete given post
const openDeletePostModal = (id) => {
    alert(`Are you sure you want to delete post with ${id}`);
}

// sets postsToShow 
const setPostsToDisplay = () => {
    blogsToShow = [];

    let postCategory = postCategoryIdx;

    postsData.forEach(b => {
        let post = JSON.parse(b.getAttribute("value"));

        if (post.title.toString().toLowerCase().includes(postSearchString) ||
            post.description.toString().toLowerCase().includes(postSearchString) ||
            post.content.toString().toLowerCase().includes(postSearchString) ||
            postSearchString.toString() === "") {
            if (post.category == postCategory || postCategory === 100) {
                blogsToShow.push(post);
            }
        }
    });

    sortPosts();
    updatePaginationVariables(blogsToShow.length);
}

// sort posts by updated date
const sortPosts = () => {
    blogsToShow = blogsToShow.sort(
        (b1, b2) => (b1.updatedOn < b2.updatedOn) ? 1 : (b1.updatedOn > b2.updatedOn) ? -1 : 0);
}

const updatePaginationVariables = (postCount) => {
    totalPageCount = Math.ceil(postCount / maxPostsPerPage);
}

const setPageNumber = (num) => {
    if (num > totalPageCount || num < 1) {
        if (num <= 1) {
            num = 1;
            currentPageNumber = 1;
        }
        else if (num >= totalPageCount) {
            num = totalPageCount;
            currentPageNumber = totalPageCount;
        }
    }

    currentPageNumber = num;
    setPostsToDisplay();
    displayPosts();
}

// displays all posts 
const displayPosts = () => {
    createPagination();
    postsDisplayContainer.innerHTML = '';

    let searchHeading = document.createElement('h3');

    if (blogsToShow.length <= 0) {
        // no results
        const divElement = document.createElement('div');
        divElement.innerHTML =
            `<div class="col-lg-6">
                <p>Sorry... There are no related results for the above query :( </p>
            </div>`;

        if (postSearchString == "") {
            divElement.innerHTML =
                `<div class="col-lg-6">
                    <p>Find any interesting blog posts? Favorite them to view them on this dashboard!</p>
                </div>`;
        } 
        postsDisplayContainer.append(searchHeading);

        postsDisplayContainer.append(divElement);
        return;
    }

    let curPostIdx = (currentPageNumber - 1) * maxPostsPerPage;
    let postIdxOnCurrentPage = currentPageNumber * maxPostsPerPage;

    for (var i = curPostIdx; i < postIdxOnCurrentPage; i++) {

        if (blogsToShow[i] == null) {

        } else {
            var updatedOnDate = new Date(blogsToShow[i].updatedOn);
            var year = updatedOnDate.getFullYear();
            var month = months[updatedOnDate.getMonth()].substring(0, 3);
            var day = updatedOnDate.getDate();

            const divElement = document.createElement('div');
            divElement.classList = 'standard-card post-card mb-4'
            divElement.innerHTML =
                `<div>
                    <a href="${postDetailsPath}/${blogsToShow[i].id}">
                        <div class="banner-tag ${getPostCategoryClass(blogsToShow[i].category)}">
                            <div>${getPostCategoryName(blogsToShow[i].category)}</div >
                        </div>
                        <img class="post-card-img" src="${getPostImagePath(blogsToShow[i].headerImage)}" alt="${blogsToShow[i].title}" />
                    </a>
                    <div class="post-card-body">
                        <h2 class="post-card-body-title"><a href="${postDetailsPath}/${blogsToShow[i].id}">${blogsToShow[i].title}</a></h2>
                        <!--<p class="post-card-body-description">${blogsToShow[i].description}</p>-->
                        <div class="post-card-body-details">
                            <span>
                                <span>
                                    <svg width="30px" height="30px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 9H21M7 3V5M17 3V5M6 13H8M6 17H8M11 13H13M11 17H13M16 13H18M16 17H18M6.2 21H17.8C18.9201 21 19.4802 21 19.908 20.782C20.2843 20.5903 20.5903 20.2843 20.782 19.908C21 19.4802 21 18.9201 21 17.8V8.2C21 7.07989 21 6.51984 20.782 6.09202C20.5903 5.71569 20.2843 5.40973 19.908 5.21799C19.4802 5 18.9201 5 17.8 5H6.2C5.0799 5 4.51984 5 4.09202 5.21799C3.71569 5.40973 3.40973 5.71569 3.21799 6.09202C3 6.51984 3 7.07989 3 8.2V17.8C3 18.9201 3 19.4802 3.21799 19.908C3.40973 20.2843 3.71569 20.5903 4.09202 20.782C4.51984 21 5.07989 21 6.2 21Z" stroke="#000000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                                    </svg>
                                </span>
                                <span>
                                    ${month}/${day}/${year}
                                </span>
                            </span>
                        </div>
                    </div>
                </div>`;

            postsDisplayContainer.append(divElement);
        }
    }

    dropdownContent = document.querySelectorAll('.dropdown-content');
    menuIcons = document.querySelectorAll('.menu-icon');
    // adds on click listener to all comment menus
    menuIcons.forEach(menu => {
        menu.addEventListener('click', () => {
            openCommentDropDownMenu(menu);
        });
    });
}

// handles pagination and creates/designs the pagination nav accordingly
const createPagination = () => {
    paginationNavContainer.innerHTML = '';
    // do pagination only if greater than maxPostsPerPage posts
    if (blogsToShow.length > maxPostsPerPage) {
        const hrElement = document.createElement('hr');
        hrElement.classList = "my-0";

        const ulElement = document.createElement('ul');
        ulElement.classList = "pagination justify-content-center my-4";

        let prevBtnElement = document.createElement('li');
        prevBtnElement.classList = "page-item";
        prevBtnElement.addEventListener('click', () => setPageNumber(--currentPageNumber));

        if (currentPageNumber == 1) {
            prevBtnElement.innerHTML = `<a class="page-link disabled">Newer</a>`;
        } else {
            prevBtnElement.innerHTML = `<a class="page-link">Newer</a>`;
        }
        ulElement.append(prevBtnElement);

        for (let i = 0; i < totalPageCount; i++) {
            let liElement = document.createElement('li');
            liElement.classList = "page-item";
            liElement.addEventListener('click', () => setPageNumber(i + 1));
            if (currentPageNumber == (i + 1)) {
                liElement.innerHTML = `<a class="page-link font-weight-bold">${i + 1}</a>`;
            } else {
                liElement.innerHTML = `<a class="page-link">${i + 1}</a>`;
            }

            ulElement.append(liElement);
        }

        let nextBtnElement = document.createElement('li');
        nextBtnElement.classList = "page-item";
        nextBtnElement.addEventListener('click', () => setPageNumber(++currentPageNumber));

        if (currentPageNumber == totalPageCount) {
            nextBtnElement.innerHTML = `<a class="page-link disabled">Older</a>`;
        } else {
            nextBtnElement.innerHTML = `<a class="page-link">Older</a>`;
        }
        ulElement.append(nextBtnElement);

        paginationNavContainer.append(hrElement);
        paginationNavContainer.append(ulElement);
    }
}

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

// create category badge on detail view dashboard
const createCategoryBadge = () => {
  let post = JSON.parse(postData.getAttribute("value"));

  const badgeElement = document.createElement('a');
  badgeElement.innerHTML = `<a class="badge text-decoration-none link-light cs">${post.category}</a>`;

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
        case 0:
            return 'technology';
        case 1:
            return 'economy-finance';
        case 2:
            return 'health-fitness';
        case 3:
            return 'food';
        case 4:
            return 'politics';
        case 5:
            return 'travel';
        case 6:
            return 'human-science';
        case 7:
            return 'nature-science';
        default:
            return 'all';
    }
}



setPostsToDisplay();
displayPosts();