// search bar and buttons refs
const searchBarInput = document.querySelector('#searchBarInput');
const searchBarBtn = document.querySelector('#searchBarBtn');

// post and category containers refs
const featuredPostContainer = document.querySelector('#featuredBlogContainer');
const postCategoryListContainer = document.querySelector('#blogCategoryListContainer');
const postsDisplayContainer = document.querySelector('#blogsDisplayContainer');
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


const getPostImagePath = (userId) => {
    return `../UserFiles/Users/${userId}/ProfilePicture`;
}

// sets postCategoryIdx
const setPostCategory = (selectedCategory) => {
    // toggle post category on/off upon clicking same category
    if (selectedCategory == getPostCategoryName(postCategoryIdx)) {
        postCategoryIdx = 100;
        clickedCat = false;
    } else {
        postCategoryIdx = getPostCategoryIdx(selectedCategory);
        console.log('hide featured Post!');
        hideFeaturedPost = true;
        clickedCat = true;
    }
    
    setPostSearchString("");
    searchBarInput.value = "";
    setPostsToDisplay();
    displayPosts();
    console.log('clicked ' + selectedCategory);
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
    console.log(postSearchString);

    if (postSearchString == '' && clickedCat == false) {
        console.log('hide featured Post!');
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

        console.log('title includes: ' + post.title.toString().toLowerCase().includes(postSearchString));
        console.log('description includes: ' + post.description.toString().toLowerCase().includes(postSearchString));
        console.log('content includes: ' + post.content.toString().toLowerCase().includes(postSearchString));

        if (post.title.toString().toLowerCase().includes(postSearchString) ||
            post.description.toString().toLowerCase().includes(postSearchString) ||
            post.content.toString().toLowerCase().includes(postSearchString) ||
            postSearchString.toString() === "") {
            if (post.category == postCategory || postCategory === 100) {
                postsToShow.push(post);
            }
        }
    });

    sortPosts();

    console.log('post search string ' + postSearchString);
    console.log('posts to show ' + postsToShow);
    console.log('posts to show length ' + postsToShow.length);
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
    console.log('display featured post: ' + featuredPost);

    var createdOnDate = new Date(featuredPost.createdOn);
    var year = createdOnDate.getFullYear();
    var month = months[createdOnDate.getMonth()].substring(0, 3);
    var day = createdOnDate.getDate();

    const divElement = document.createElement('div');
    divElement.innerHTML =
        `<div class="card mb-4">
                <a href="${postDetailsPath}/${featuredPost.id}">
                        <div class="banner-tag ${getPostCategoryClass(featuredPost.category)}">
                            <div>${getPostCategoryName(featuredPost.category)}</div >
                        </div>
                        <img class="card-img-top featured-img" src="${getPostImagePath(featuredPost.createdBy.id)}/${featuredPost.id}/HeaderImage.jpg" alt="${featuredPost.title}" />
                    </a>
                <div class="card-body text-left">
                    <div class="small text-muted">Last Updated on ${month} ${day}, ${year} by ${featuredPost.createdBy.userName}</div>
                    <h2 class="card-title h4"><a href="${postDetailsPath}/${featuredPost.id}">${featuredPost.title}</a></h2>
                    <p class="card-text text-truncate">${featuredPost.description}</p>
                </div>
            </div>`;

    featuredPostContainer.appendChild(divElement);
}

// displays all posts
const displayPosts = () => {
    createPagination();

    postsDisplayContainer.innerHTML = '';

    let searchHeading = document.createElement('h3');
    if (postSearchString == "" && clickedCat == false) {
        searchHeading.innerHTML = ``;
    } else if (postSearchString == "" && clickedCat) {
        searchHeading.innerHTML = `Search Results: ${getPostCategoryName(postCategoryIdx)}`;
    } else {
        searchHeading.innerHTML = `Search Results: ${getPostCategoryName(postCategoryIdx)} - ${postSearchString}`;
    }
    postsDisplayContainer.append(searchHeading);

    if (postsToShow.length <= 0) {
        // no results
        const divElement = document.createElement('div');
        divElement.innerHTML =
            `<div class="col-lg-6">
                <p>Sorry... There are no related results for the above query :( </p>
            </div>`;

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
        divElement.classList = "col-lg-6";
        divElement.innerHTML =
           `<div class="card mb-4">
                    <a href="${postDetailsPath}/${postsToShow[i].id}">
                        <div class="banner-tag ${getPostCategoryClass(postsToShow[i].category)}">
                            <div>${getPostCategoryName(postsToShow[i].category)}</div >
                        </div>
                        <img class="card-img-top" src="${getPostImagePath(featuredPost.createdBy.id) }/${postsToShow[i].id}/HeaderImage.jpg" alt="${postsToShow[i].title}" />
                    </a>
                    <div class="card-body text-left">
                        <div class="small text-muted">Last Updated on ${month} ${day}, ${year} by ${postsToShow[i].createdBy.userName}</div>
                        <h2 class="card-title h4"><a href="${postDetailsPath}/${postsToShow[i].id}">${postsToShow[i].title}</a></h2>
                        <p class="card-text text-truncate">${postsToShow[i].description}</p>
                    </div>
                </div>`;

        postsDisplayContainer.append(divElement);
    }
}

// handles pagination and creates/designs the pagination nav accordingly
const createPagination = () => {
    paginationNavContainer.innerHTML = '';
    // do pagination only if greater than 10 posts
    if (postsToShow.length > maxPostsPerPage) {

        console.log('posts length: ' + postsToShow.length);
        console.log('max posts: ' + maxPostsPerPage);
        console.log('total page count: ' + totalPageCount);

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
const getPostCategoryIdx = (value) => {
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
const getPostCategoryClass = (value) => {
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

setPostCategory("All");
setPostSearchString("")
setUpPostCategoryList();
setPostsToDisplay();
displayPosts();






