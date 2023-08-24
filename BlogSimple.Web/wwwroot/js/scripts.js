// search bar and buttons refs
const searchBarInput = document.querySelector('#searchBarInput');
const searchBarBtn = document.querySelector('#searchBarBtn');

// blog and category containers refs
const featuredBlogContainer = document.querySelector('#featuredBlogContainer');
const blogCategoryListContainer = document.querySelector('#blogCategoryListContainer');
const blogsDisplayContainer = document.querySelector('#blogsDisplayContainer');
const paginationNavContainer = document.querySelector('#paginationNavContainer');

var featuredBlog = JSON.parse(document.querySelector('#featuredBlog').getAttribute("value"));
var blogsData = document.querySelectorAll('div.blogsData');
var blogCategoryData = JSON.parse(document.querySelector('#blogCategoryData').getAttribute("value"));

// access to all the blogs in provided in view model
let blogsToShow = [];

// pagination values 
const maxBlogsPerPage = 2;
let currentPageNumber = 1;

// search string and category
let blogSearchString = "";
let blogCategoryIdx = 100;

const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

// controller paths
let blogDetailsPath = '/Blog/Details';
let blogImagePath = '../UserFiles/Blogs';

// sets blogCategoryIdx
const setBlogCategory = (selectedCategory) => {
    // toggle blog category on/off upon clicking same category
    if (selectedCategory == getBlogCategoryName(blogCategoryIdx)) {
        blogCategoryIdx = 100;
    } else {
        blogCategoryIdx = getBlogCategoryIdx(selectedCategory);
    }
    
    setBlogSearchString("");
    searchBarInput.value = "";
    setBlogsToDisplay();
    displayBlogs();
    console.log('clicked ' + selectedCategory);
}

// lists all blog cateogries on side widget
const setUpBlogCategoryList = () => {
    blogCategoryData.forEach(cat => {
        var catName = getBlogCategoryName(cat);

        const listItemElement = document.createElement('li');
        listItemElement.innerHTML = `<a href="#" onclick="return false;">${catName}</a>`;
        listItemElement.classList = 'list-select';
        listItemElement.addEventListener('click', () => setBlogCategory(catName));

        blogCategoryListContainer.append(listItemElement);
    });
}

// sets blogSearchString 
const setBlogSearchString = (searchString) => {
    blogSearchString = searchString;
    currentPageNumber = 1;
}

// sets blogsToShow 
const setBlogsToDisplay = () => {
    blogsToShow = [];

    let blogCategory = blogCategoryIdx;

    blogsData.forEach(b => {
        let blog = JSON.parse(b.getAttribute("value"));

        // exclude featured blogs
        if (blog.isFeatured) return;

        if (blog.title.toString().toLowerCase().includes(blogSearchString) ||
            blog.description.toString().toLowerCase().includes(blogSearchString) ||
            blog.content.toString().toLowerCase().includes(blogSearchString) ||
            blogSearchString.toString() === "") {
            if (blog.category == blogCategory || blogCategory === 100) {
                blogsToShow.push(blog);
            }
        }
    });

    sortBlogs();

    console.log('blog search string ' + blogSearchString);
    console.log('blogs to show ' + blogsToShow);
    console.log('blogs to show length ' + blogsToShow.length);
    updatePaginationVariables(blogsToShow.length);
}

// sort blogs by updated date
const sortBlogs = () => {
    blogsToShow = blogsToShow.sort(
        (b1, b2) => (b1.updatedOn < b2.updatedOn) ? 1 : (b1.updatedOn > b2.updatedOn) ? -1 : 0);
}

const updatePaginationVariables = (blogCount) => {
    totalPageCount = Math.ceil(blogCount / maxBlogsPerPage);
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
    setBlogsToDisplay();
    displayBlogs();
}

// displays the featured blog 
const displayFeaturedBlog = () => {
    if (featuredBlog.id == null) return;
    console.log('display featured blog: ' + featuredBlog);

    var createdOnDate = new Date(featuredBlog.createdOn);
    var year = createdOnDate.getFullYear();
    var month = months[createdOnDate.getMonth()].substring(0, 3);
    var date = createdOnDate.getDate();
    var date = createdOnDate.getDate();

    const divElement = document.createElement('div');
    divElement.innerHTML =
        `<div class="card mb-4">
                <a href="${blogDetailsPath}/${featuredBlog.id}">
                        <div class="banner-tag ${getBlogCategoryClass(featuredBlog.category)}">
                            <div>${getBlogCategoryName(featuredBlog.category)}</div >
                        </div>
                        <img class="card-img-top" src="${blogImagePath}/${featuredBlog.id}/HeaderImage.jpg" alt="${featuredBlog.title}" />
                    </a>
                <div class="card-body">
                    <div class="small text-muted">Last updated on ${month} ${date}, ${year}</div>
                    <h2 class="card-title h4"><a href="${blogDetailsPath}/${featuredBlog.id}">${featuredBlog.title}</a></h2>
                    <p class="card-text text-truncate">${featuredBlog.description}</p>
                </div>
            </div>`;

    featuredBlogContainer.appendChild(divElement);
}

// displays all blogs 
const displayBlogs = () => {
    createPagination();

    blogsDisplayContainer.innerHTML = '';

    let searchHeading = document.createElement('h3');
    if (blogSearchString == "") {
        searchHeading.innerHTML = `Search Results: ${getBlogCategoryName(blogCategoryIdx)}`;
    } else {
        searchHeading.innerHTML = `Search Results: ${getBlogCategoryName(blogCategoryIdx)} - ${blogSearchString}`;
    }
    blogsDisplayContainer.append(searchHeading);

    if (blogsToShow.length <= 0) {
        // no results
        const divElement = document.createElement('div');
        divElement.innerHTML =
            `<div class="col-lg-6">
                <p>Sorry... There are no related results for the above query :( </p>
            </div>`;

        blogsDisplayContainer.append(divElement);
        return;
    }

    let curBlogIdx = (currentPageNumber - 1) * maxBlogsPerPage;
    let blogIdxOnCurrentPage = currentPageNumber * maxBlogsPerPage;

    for (var i = curBlogIdx; i < blogIdxOnCurrentPage; i++) {

        if (blogsToShow[i] == null) return;
        if (blogsToShow[i].isFeatured) return;

        var createdOnDate = new Date(blogsToShow[i].createdOn);
        var year = createdOnDate.getFullYear();
        var month = months[createdOnDate.getMonth()].substring(0, 3);
        var day = createdOnDate.getDate();

        const divElement = document.createElement('div');
        divElement.classList = "col-lg-6";
        divElement.innerHTML =
           `<div class="card mb-4">
                    <a href="${blogDetailsPath}/${blogsToShow[i].id}">
                        <div class="banner-tag ${getBlogCategoryClass(blogsToShow[i].category)}">
                            <div>${getBlogCategoryName(blogsToShow[i].category)}</div >
                        </div>
                        <img class="card-img-top" src="${blogImagePath}/${blogsToShow[i].id}/HeaderImage.jpg" alt="${blogsToShow[i].title}" />
                    </a>
                    <div class="card-body">
                        <div class="small text-muted">Last updated on ${month} ${day}, ${year}</div>
                        <h2 class="card-title h4"><a href="${blogDetailsPath}/${blogsToShow[i].id}">${blogsToShow[i].title}</a></h2>
                        <p class="card-text text-truncate">${blogsToShow[i].description}</p>
                    </div>
                </div>`;

        blogsDisplayContainer.append(divElement);
    }
}

// handles pagination and creates/designs the pagination nav accordingly
const createPagination = () => {
    paginationNavContainer.innerHTML = '';
    // do pagination only if greater than 10 blogs
    if (blogsToShow.length > maxBlogsPerPage) {

        console.log('blogs length: ' + blogsToShow.length);
        console.log('max blogs: ' + maxBlogsPerPage);
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
    setBlogSearchString(e.target.value);
    setBlogsToDisplay();
    displayBlogs();
});



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

setBlogCategory("All");
setBlogSearchString("")
setUpBlogCategoryList();
setBlogsToDisplay();
displayBlogs();
displayFeaturedBlog();






