// post menu refs
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
            `<div class="col-lg-12">
                <p>Sorry... There are no related results for the above query :( </p>
            </div>`;

        if (postSearchString == "") {
            divElement.innerHTML =
                `<div class="col-lg-12">
                    <p>Click <a href="${createPostPath}">here</a> to create your first post!</p>
                </div>`;
        } else {
            searchHeading.innerHTML = `Search Results: ${postSearchString}`;
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

            let eyeSVG = '<svg width="50px" height="50px" viewBox="-2.4 -2.4 28.80 28.80" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M4 4L20 20" stroke="#000000" stroke-width="1.2" stroke-linecap="round"></path> <path fill-rule="evenodd" clip-rule="evenodd" d="M6.22308 5.63732C4.19212 6.89322 2.60069 8.79137 1.73175 11.0474C1.49567 11.6604 1.49567 12.3396 1.73175 12.9526C3.31889 17.0733 7.31641 20 12 20C14.422 20 16.6606 19.2173 18.4773 17.8915L17.042 16.4562C15.6033 17.4309 13.8678 18 12 18C8.17084 18 4.89784 15.6083 3.5981 12.2337C3.54022 12.0835 3.54022 11.9165 3.5981 11.7663C4.36731 9.76914 5.82766 8.11625 7.6854 7.09964L6.22308 5.63732ZM9.47955 8.89379C8.5768 9.6272 7.99997 10.7462 7.99997 12C7.99997 14.2091 9.79083 16 12 16C13.2537 16 14.3728 15.4232 15.1062 14.5204L13.6766 13.0908C13.3197 13.6382 12.7021 14 12 14C10.8954 14 9.99997 13.1046 9.99997 12C9.99997 11.2979 10.3618 10.6802 10.9091 10.3234L9.47955 8.89379ZM15.9627 12.5485L11.4515 8.03729C11.6308 8.0127 11.8139 8 12 8C14.2091 8 16 9.79086 16 12C16 12.1861 15.9873 12.3692 15.9627 12.5485ZM18.5678 15.1536C19.3538 14.3151 19.9812 13.3259 20.4018 12.2337C20.4597 12.0835 20.4597 11.9165 20.4018 11.7663C19.1021 8.39172 15.8291 6 12 6C11.2082 6 10.4402 6.10226 9.70851 6.29433L8.11855 4.70437C9.32541 4.24913 10.6335 4 12 4C16.6835 4 20.681 6.92668 22.2682 11.0474C22.5043 11.6604 22.5043 12.3396 22.2682 12.9526C21.7464 14.3074 20.964 15.5331 19.9824 16.5682L18.5678 15.1536Z" fill="#000000"></path> </g></svg>';
            if (blogsToShow[i].isPublished) {
                eyeSVG = '<svg width="50px" height="50px" viewBox="-2.4 -2.4 28.80 28.80" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M21.335 11.4069L22.2682 11.0474L21.335 11.4069ZM21.335 12.5932L20.4018 12.2337L21.335 12.5932ZM2.66492 11.4068L1.73175 11.0474L2.66492 11.4068ZM2.66492 12.5932L1.73175 12.9526L2.66492 12.5932ZM3.5981 11.7663C4.89784 8.39171 8.17084 6 12 6V4C7.31641 4 3.31889 6.92667 1.73175 11.0474L3.5981 11.7663ZM12 6C15.8291 6 19.1021 8.39172 20.4018 11.7663L22.2682 11.0474C20.681 6.92668 16.6835 4 12 4V6ZM20.4018 12.2337C19.1021 15.6083 15.8291 18 12 18V20C16.6835 20 20.681 17.0733 22.2682 12.9526L20.4018 12.2337ZM12 18C8.17084 18 4.89784 15.6083 3.5981 12.2337L1.73175 12.9526C3.31889 17.0733 7.31641 20 12 20V18ZM20.4018 11.7663C20.4597 11.9165 20.4597 12.0835 20.4018 12.2337L22.2682 12.9526C22.5043 12.3396 22.5043 11.6604 22.2682 11.0474L20.4018 11.7663ZM1.73175 11.0474C1.49567 11.6604 1.49567 12.3396 1.73175 12.9526L3.5981 12.2337C3.54022 12.0835 3.54022 11.9165 3.5981 11.7663L1.73175 11.0474Z" fill="#000000"></path> <circle cx="12" cy="12" r="3" stroke="#000000" stroke-width="1.2" stroke-linecap="round" stroke-linejoin="round"></circle> </g></svg>';
            }

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
                    <div class="card-footer">
                        ${eyeSVG}
                        <div class="dropdown">
                            <svg class="menu-icon" width="60px" height="40px" viewBox="-2.4 -2.4 28.80 28.80" fill="none" xmlns="http://www.w3.org/2000/svg" transform="rotate(90)matrix(1, 0, 0, 1, 0, 0)"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M19 13C19.5523 13 20 12.5523 20 12C20 11.4477 19.5523 11 19 11C18.4477 11 18 11.4477 18 12C18 12.5523 18.4477 13 19 13Z" stroke="#000000" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M12 13C12.5523 13 13 12.5523 13 12C13 11.4477 12.5523 11 12 11C11.4477 11 11 11.4477 11 12C11 12.5523 11.4477 13 12 13Z" stroke="#000000" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M5 13C5.55228 13 6 12.5523 6 12C6 11.4477 5.55228 11 5 11C4.44772 11 4 11.4477 4 12C4 12.5523 4.44772 13 5 13Z" stroke="#000000" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>
                            <div class="dropdown-content">
                                <a href="${editPostPath}/${blogsToShow[i].id}">Edit</a>
                                <a href="${deletePostPath}/${blogsToShow[i].id}">Delete</a>
                            </div>
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


setPostsToDisplay();
displayPosts();