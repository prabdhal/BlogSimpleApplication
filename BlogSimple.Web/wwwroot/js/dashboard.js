// search bar and buttons refs
const searchBarInput = document.querySelector('#searchBarInput');
const searchBarBtn = document.querySelector('#searchBarBtn');

// blog and category containers refs
const blogsDisplayContainer = document.querySelector('#blogsDisplayContainer');
const blogCategoryListContainer = document.querySelector('#blogCategoryListContainer');
const paginationNavContainer = document.querySelector('#paginationNavContainer');
const categoryBadgeContainer = document.querySelector('#categoryBadgeContainer');

var blogData = document.querySelector('div.blogData');
var blogsData = document.querySelectorAll('div.blogsData');
var blogCategoryData = JSON.parse(document.querySelector('#blogCategoryData').getAttribute("value"));

// pagination values 
const maxBlogsPerPage = 10;
let currentPageNumber = 1;

// search string and category
let blogSearchString = "";
let blogCategoryIdx = 100;

const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];


// create category badge on detail view dashboard
const createCategoryBadge = () => {
    let blog = JSON.parse(blogData.getAttribute("value"));

    const badgeElement = document.createElement('a');
    badgeElement.innerHTML = `<a class="badge text-decoration-none link-light cs">${blog.category}</a>`;

    categoryBadgeContainer.append(badgeElement);
}

// opens modal to delete given blog
const openDeleteBlogModal = (id) => {
    console.log('open delete Modal');
    alert(`Are you sure you want to delete blog with ${id}`);
}


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
    console.log('blog search string ' + blogSearchString);
    console.log('blogs to show ' + blogsToShow);
    console.log('blogs to show length ' + blogsToShow.length);
    updatePaginationVariables(blogsToShow.length);
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

// displays all blogs 
const displayBlogs = () => {
    createPagination();

    blogsDisplayContainer.innerHTML = '';

    let searchHeading = document.createElement('h3');
    if (blogSearchString == "") {
    } else {
        searchHeading.innerHTML = `Search Results: ${blogSearchString}`;
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

        var updatedOnDate = new Date(blogsToShow[i].updatedOn);
        var year = updatedOnDate.getFullYear();
        var month = months[updatedOnDate.getMonth()].substring(0, 3);
        var day = updatedOnDate.getDate();

        let eyeSVG = '<svg width="50px" height="50px" viewBox="-2.4 -2.4 28.80 28.80" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M4 4L20 20" stroke="#000000" stroke-width="1.2" stroke-linecap="round"></path> <path fill-rule="evenodd" clip-rule="evenodd" d="M6.22308 5.63732C4.19212 6.89322 2.60069 8.79137 1.73175 11.0474C1.49567 11.6604 1.49567 12.3396 1.73175 12.9526C3.31889 17.0733 7.31641 20 12 20C14.422 20 16.6606 19.2173 18.4773 17.8915L17.042 16.4562C15.6033 17.4309 13.8678 18 12 18C8.17084 18 4.89784 15.6083 3.5981 12.2337C3.54022 12.0835 3.54022 11.9165 3.5981 11.7663C4.36731 9.76914 5.82766 8.11625 7.6854 7.09964L6.22308 5.63732ZM9.47955 8.89379C8.5768 9.6272 7.99997 10.7462 7.99997 12C7.99997 14.2091 9.79083 16 12 16C13.2537 16 14.3728 15.4232 15.1062 14.5204L13.6766 13.0908C13.3197 13.6382 12.7021 14 12 14C10.8954 14 9.99997 13.1046 9.99997 12C9.99997 11.2979 10.3618 10.6802 10.9091 10.3234L9.47955 8.89379ZM15.9627 12.5485L11.4515 8.03729C11.6308 8.0127 11.8139 8 12 8C14.2091 8 16 9.79086 16 12C16 12.1861 15.9873 12.3692 15.9627 12.5485ZM18.5678 15.1536C19.3538 14.3151 19.9812 13.3259 20.4018 12.2337C20.4597 12.0835 20.4597 11.9165 20.4018 11.7663C19.1021 8.39172 15.8291 6 12 6C11.2082 6 10.4402 6.10226 9.70851 6.29433L8.11855 4.70437C9.32541 4.24913 10.6335 4 12 4C16.6835 4 20.681 6.92668 22.2682 11.0474C22.5043 11.6604 22.5043 12.3396 22.2682 12.9526C21.7464 14.3074 20.964 15.5331 19.9824 16.5682L18.5678 15.1536Z" fill="#000000"></path> </g></svg>';
        if (blogsToShow[i].isPublished) {
            eyeSVG = '<svg width="50px" height="50px" viewBox="-2.4 -2.4 28.80 28.80" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M21.335 11.4069L22.2682 11.0474L21.335 11.4069ZM21.335 12.5932L20.4018 12.2337L21.335 12.5932ZM2.66492 11.4068L1.73175 11.0474L2.66492 11.4068ZM2.66492 12.5932L1.73175 12.9526L2.66492 12.5932ZM3.5981 11.7663C4.89784 8.39171 8.17084 6 12 6V4C7.31641 4 3.31889 6.92667 1.73175 11.0474L3.5981 11.7663ZM12 6C15.8291 6 19.1021 8.39172 20.4018 11.7663L22.2682 11.0474C20.681 6.92668 16.6835 4 12 4V6ZM20.4018 12.2337C19.1021 15.6083 15.8291 18 12 18V20C16.6835 20 20.681 17.0733 22.2682 12.9526L20.4018 12.2337ZM12 18C8.17084 18 4.89784 15.6083 3.5981 12.2337L1.73175 12.9526C3.31889 17.0733 7.31641 20 12 20V18ZM20.4018 11.7663C20.4597 11.9165 20.4597 12.0835 20.4018 12.2337L22.2682 12.9526C22.5043 12.3396 22.5043 11.6604 22.2682 11.0474L20.4018 11.7663ZM1.73175 11.0474C1.49567 11.6604 1.49567 12.3396 1.73175 12.9526L3.5981 12.2337C3.54022 12.0835 3.54022 11.9165 3.5981 11.7663L1.73175 11.0474Z" fill="#000000"></path> <circle cx="12" cy="12" r="3" stroke="#000000" stroke-width="1.2" stroke-linecap="round" stroke-linejoin="round"></circle> </g></svg>';
        }


        const divElement = document.createElement('div');
        divElement.classList = "col-lg-6";
        divElement.innerHTML =
            `<!-- Blog post-->
                <div class="card mb-4">
                    <a href="/Blog/Details/${blogsToShow[i].id}">
                        <div class="banner-tag ${getBlogCategoryClass(blogsToShow[i].category)}">
                            <div>${getBlogCategoryName(blogsToShow[i].category)}</div >
                        </div>
                        <img class="card-img-top" src="https://dummyimage.com/700x350/dee2e6/6c757d.jpg" alt="..." />
                    </a>
                    <div class="card-body">
                        <h2 class="card-title h4"><a href="/Blog/Details/${blogsToShow[i].id}">${blogsToShow[i].title}</a></h2>
                        <div class="small text-muted">Last Updated on ${month} ${day}, ${year}</div>
                        <p class="card-text text-truncate">${blogsToShow[i].description}</p>
                    </div>
                    <div class="card-footer">
                        ${eyeSVG}

                        <a href="/Blog/Edit/${blogsToShow[i].id}">
                            <svg width="50px" height="50px" viewBox="-2.4 -2.4 28.80 28.80" fill="none" xmlns="http://www.w3.org/2000/svg" stroke="#f5f5f5" transform="rotate(0)matrix(1, 0, 0, 1, 0, 0)"><g id="SVGRepo_bgCarrier" stroke-width="0" transform="translate(0,0), scale(1)"><rect x="-2.4" y="-2.4" width="28.80" height="28.80" rx="14.4" fill="#1786fd" strokewidth="0"></rect></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round" stroke="#a01c0e" stroke-width="0.24000000000000005"></g><g id="SVGRepo_iconCarrier"> <path d="M18.3785 8.44975L8.9636 17.8648C8.6844 18.144 8.3288 18.3343 7.94161 18.4117L4.99988 19.0001L5.58823 16.0583C5.66566 15.6711 5.85597 15.3155 6.13517 15.0363L15.5501 5.62132M18.3785 8.44975L19.7927 7.03553C20.1832 6.64501 20.1832 6.01184 19.7927 5.62132L18.3785 4.20711C17.988 3.81658 17.3548 3.81658 16.9643 4.20711L15.5501 5.62132M18.3785 8.44975L15.5501 5.62132" stroke="#f5f5f5" stroke-width="1.2" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>    
                        </a>
                        <svg width="50px" height="50px" onclick="openDeleteBlogModal(${blogsToShow[i].id})" viewBox="-2.4 -2.4 28.80 28.80" fill="none" xmlns="http://www.w3.org/2000/svg" stroke="#ffffff"><g id="SVGRepo_bgCarrier" stroke-width="0"><rect x="-2.4" y="-2.4" width="28.80" height="28.80" rx="14.4" fill="#1786fd" strokewidth="0"></rect></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M10 11V17" stroke="#ffffff" stroke-width="1.2" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M14 11V17" stroke="#ffffff" stroke-width="1.2" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M4 7H20" stroke="#ffffff" stroke-width="1.2" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M6 7H12H18V18C18 19.6569 16.6569 21 15 21H9C7.34315 21 6 19.6569 6 18V7Z" stroke="#ffffff" stroke-width="1.2" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M9 5C9 3.89543 9.89543 3 11 3H13C14.1046 3 15 3.89543 15 5V7H9V5Z" stroke="#ffffff" stroke-width="1.2" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>
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

window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

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
//setUpBlogCategoryList();
setBlogsToDisplay();
displayBlogs();



