// search bar and buttons refs
const searchBarInput = document.querySelector('#searchBarInput');
const searchBarBtn = document.querySelector('#searchBarBtn');

// post and category containers refs
const featuredPostContainer = document.querySelector('#featuredBlogContainer');
const postCategoryListContainer = document.querySelector('#blogCategoryListContainer');
const postsDisplayContainer = document.querySelector('#blogsDisplayContainer');
const featuredBlogHeader = document.querySelector('#featuredBlogHeader');
const paginationNavContainer = document.querySelector('#paginationNavContainer');

let featuredPost = JSON.parse(document.querySelector('#featuredBlog').getAttribute("value"));
let postsData = document.querySelectorAll('div.blogsData');
let postCategoryData = JSON.parse(document.querySelector('#blogCategoryData').getAttribute("value"));

// access to all the posts in provided in view model
let postsToShow = [];

// pagination values 
const maxPostsPerPage = 6;
let currentPageNumber = 1;

// search string and category
let postSearchString = "";
let postCategoryIdx = 100;

let hideFeaturedPost = false;
let clickedCat = false;

const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

// controller paths
let postDetailsPath = '/Post/PostDetails';

const getImagePath = (img) => {
    return `data:image/jpg;base64,${img}`;
}

// sets postCategoryIdx
const setPostCategory = (selectedCategory) => {
    // toggle post category on/off upon clicking same category
    if (selectedCategory == getPostCategoryName(postCategoryIdx)) {
        postCategoryIdx = 100;
        clickedCat = false;
    } else {
        // Hide featured post
        postCategoryIdx = getPostCategoryIdx(selectedCategory);
        hideFeaturedPost = true;
        clickedCat = true;
    }

    // Search category clicked
    setPostSearchString("");
    searchBarInput.value = "";
    setPostsToDisplay();
    displayPosts();
}

// lists all post cateogries on side widget
const setUpPostCategoryList = () => {
    postCategoryData.forEach(cat => {
        var catName = getPostCategoryName(cat);

        const listItemElement = document.createElement('li');
        listItemElement.innerHTML = `<a href="#" onclick="return false;">${catName}</a>`;
        listItemElement.classList = 'list-select';
        listItemElement.addEventListener('click', () => setPostCategory(catName));

        postCategoryListContainer.append(listItemElement);
    });
}

// sets postSearchString 
const setPostSearchString = (searchString) => {
    postSearchString = searchString.toLowerCase();
    hideFeaturedPost = false;

    // Hides featured post
    if (postSearchString == '' && clickedCat == false) {
        hideFeaturedPost = true;
    }

  currentPageNumber = 1;
}

// sets postsToShow 
const setPostsToDisplay = () => {
    postsToShow = [];

    let postCategory = postCategoryIdx;

    displayFeaturedPost();

    postsData.forEach(p => {
        let post = JSON.parse(p.getAttribute("value"));

        // exclude featured posts
        if (post.isFeatured && hideFeaturedPost) return;

        // Displays posts that contain the entered search string in their title, description or category
        if (post.title.toString().toLowerCase().includes(postSearchString) ||
            post.description.toString().toLowerCase().includes(postSearchString) ||
            getPostCategoryName(post.category).toString().toLowerCase().includes(postSearchString) ||
            postSearchString.toString() === "") {
            if (post.category == postCategory || postCategory === 100) {
                postsToShow.push(post);
            }
        }
    });

    sortPosts();
    updatePaginationVariables(postsToShow.length);
}

// sort posts by updated date
const sortPosts = () => {
    postsToShow = postsToShow.sort(
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

// displays the featured post 
const displayFeaturedPost = () => {

    featuredPostContainer.innerHTML = '';

    if (featuredPost.id == null || hideFeaturedPost == false) return;

    // Displays featured post
    var createdOnDate = new Date(featuredPost.createdOn);
    var year = createdOnDate.getFullYear();
    var month = months[createdOnDate.getMonth()].substring(0, 3);
    var day = createdOnDate.getDate();

    const divElement = document.createElement('div');
    divElement.innerHTML =
        `<div class="featured-post-card">
            <a href="${postDetailsPath}/${featuredPost.id}">
                <div class="banner-tag ${getPostCategoryClass(featuredPost.category)}">
                    <div>${getPostCategoryName(featuredPost.category)}</div >
                </div>
                <img class="featured-post-card-img" src="${getImagePath(featuredPost.headerImage)}" alt="${featuredPost.title}" />
            </a>
            <div class="featured-post-card-body">
                <h2 class="featured-post-card-body-title"><a href="${postDetailsPath}/${featuredPost.id}">${featuredPost.title}</a></h2>
                <p class="featured-post-card-body-description">${featuredPost.description}</p>
                <div class="featured-post-card-body-details">
                    <span>
                        <span>
                            <svg width="30px" height="30px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path d="M3 9H21M7 3V5M17 3V5M6 13H8M6 17H8M11 13H13M11 17H13M16 13H18M16 17H18M6.2 21H17.8C18.9201 21 19.4802 21 19.908 20.782C20.2843 20.5903 20.5903 20.2843 20.782 19.908C21 19.4802 21 18.9201 21 17.8V8.2C21 7.07989 21 6.51984 20.782 6.09202C20.5903 5.71569 20.2843 5.40973 19.908 5.21799C19.4802 5 18.9201 5 17.8 5H6.2C5.0799 5 4.51984 5 4.09202 5.21799C3.71569 5.40973 3.40973 5.71569 3.21799 6.09202C3 6.51984 3 7.07989 3 8.2V17.8C3 18.9201 3 19.4802 3.21799 19.908C3.40973 20.2843 3.71569 20.5903 4.09202 20.782C4.51984 21 5.07989 21 6.2 21Z" stroke="#000000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                            </svg>
                        </span>
                        <span style="font-style:italic;">
                            ${month}. ${day}, ${year}
                        </span>
                    </span>
                    <span>
                        <span>
                            <img class="post-creator-img" src="${getImagePath(featuredPost.createdBy.profilePicture)}" alt="${featuredPost.createdBy.userName} Profile Picture" />
                            <!--<svg width="30px" height="30px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <path d="M16 7C16 9.20914 14.2091 11 12 11C9.79086 11 8 9.20914 8 7C8 4.79086 9.79086 3 12 3C14.2091 3 16 4.79086 16 7Z" stroke="#000000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                                <path d="M12 14C8.13401 14 5 17.134 5 21H19C19 17.134 15.866 14 12 14Z" stroke="#000000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                            </svg>-->
                        </span>
                        <span>
                            <a href="/Account/Author/${featuredPost.createdBy.id}">${featuredPost.createdBy.userName}</a>
                        </span>
                    </span>
                </div>
            </div>
        </div>`;

    featuredPostContainer.appendChild(divElement);
}

// displays all posts
const displayPosts = () => {
    createPagination();

    postsDisplayContainer.innerHTML = '';
    featuredBlogHeader.innerHTML = '';

    let searchHeading = document.createElement('h3');
    if (postSearchString == "" && clickedCat == false) {
        searchHeading.innerHTML = ``;
    } else if (postSearchString == "" && clickedCat) {
        searchHeading.innerHTML = `Search Results: ${getPostCategoryName(postCategoryIdx)}`;
    } else {
        searchHeading.innerHTML = `Search Results: ${getPostCategoryName(postCategoryIdx)} - ${postSearchString}`;
    }
    featuredBlogHeader.append(searchHeading);

    if (postsToShow.length <= 0) {
        // no results
        const divElement = document.createElement('div');
        divElement.innerHTML =
            `<p>Sorry... There are no related results for the above query :( </p>`;

        postsDisplayContainer.append(divElement);
        return;
    }

    let curPostIdx = (currentPageNumber - 1) * maxPostsPerPage;
    let postIdxOnCurrentPage = currentPageNumber * maxPostsPerPage;

    for (var i = curPostIdx; i < postIdxOnCurrentPage; i++) {

        if (postsToShow[i] == null) return;
        if (postsToShow[i].isFeatured && hideFeaturedPost) return;

        var createdOnDate = new Date(postsToShow[i].createdOn);
        var year = createdOnDate.getFullYear();
        var month = months[createdOnDate.getMonth()].substring(0, 3);
        var day = createdOnDate.getDate();

        const divElement = document.createElement('div');
        divElement.classList = 'post-card mb-4'
        divElement.innerHTML =
            `<div class="post-card">
                <a href="${postDetailsPath}/${postsToShow[i].id}">
                    <div class="banner-tag ${getPostCategoryClass(postsToShow[i].category)}">
                        <div>${getPostCategoryName(postsToShow[i].category)}</div >
                    </div>
                    <img class="post-card-img" src="${getImagePath(postsToShow[i].headerImage)}" alt="${postsToShow[i].title}" />
                </a>
                <div class="post-card-body">
                    <h2 class="post-card-body-title"><a href="${postDetailsPath}/${postsToShow[i].id}">${postsToShow[i].title}</a></h2>
                    <!--<p class="post-card-body-description">${postsToShow[i].description}</p>-->
                    <div class="post-card-body-details">
                        <span>
                            <span>
                                <svg width="30px" height="30px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path d="M3 9H21M7 3V5M17 3V5M6 13H8M6 17H8M11 13H13M11 17H13M16 13H18M16 17H18M6.2 21H17.8C18.9201 21 19.4802 21 19.908 20.782C20.2843 20.5903 20.5903 20.2843 20.782 19.908C21 19.4802 21 18.9201 21 17.8V8.2C21 7.07989 21 6.51984 20.782 6.09202C20.5903 5.71569 20.2843 5.40973 19.908 5.21799C19.4802 5 18.9201 5 17.8 5H6.2C5.0799 5 4.51984 5 4.09202 5.21799C3.71569 5.40973 3.40973 5.71569 3.21799 6.09202C3 6.51984 3 7.07989 3 8.2V17.8C3 18.9201 3 19.4802 3.21799 19.908C3.40973 20.2843 3.71569 20.5903 4.09202 20.782C4.51984 21 5.07989 21 6.2 21Z" stroke="#000000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                                </svg>
                            </span>
                            <span style="font-style:italic;">
                                ${month}. ${day}, ${year}
                            </span>
                        </span>
                        <span>
                            <span>
                                <img class="post-creator-img" src="${getImagePath(postsToShow[i].createdBy.profilePicture)}" alt="${postsToShow[i].createdBy.userName} Profile Picture" />
                                <!--<svg width="20px" height="20px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <path d="M16 7C16 9.20914 14.2091 11 12 11C9.79086 11 8 9.20914 8 7C8 4.79086 9.79086 3 12 3C14.2091 3 16 4.79086 16 7Z" stroke="#000000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                                    <path d="M12 14C8.13401 14 5 17.134 5 21H19C19 17.134 15.866 14 12 14Z" stroke="#000000" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                                </svg>-->
                            </span>
                            <span>
                                <a href="/Account/Author/${postsToShow[i].createdBy.id}">${postsToShow[i].createdBy.userName}</a>
                            </span>
                        </span>
                    </div>
                </div>
            </div>`;

        postsDisplayContainer.append(divElement);
    }
}


// handles pagination and creates/designs the pagination nav accordingly
const createPagination = () => {
    paginationNavContainer.innerHTML = '';
    // Do pagination only if greater than 10 posts
    if (postsToShow.length > maxPostsPerPage) {
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

// search bar event listener 
searchBarInput.addEventListener('input', (e) => {
    setPostSearchString(e.target.value);
    setPostsToDisplay();
    displayPosts();
});

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



setPostCategory("All");
setPostSearchString("")
setUpPostCategoryList();
setPostsToDisplay();
displayPosts();






