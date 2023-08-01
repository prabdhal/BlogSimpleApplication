const searchBarInput = document.querySelector('#searchBarInput');
const searchBarBtn = document.querySelector('#searchBarBtn');

const featuredBlogContainer = document.querySelector('#featuredBlogContainer');
const blogCategoryListContainer = document.querySelector('#blogCategoryListContainer');
const blogsDisplayContainer = document.querySelector('#blogsDisplayContainer');

const htmlLabel = document.querySelector('#htmlLabel');
const cssLabel = document.getElementById('#cssLabel');
const jsLabel = document.getElementById('#jsLabel');
const cSharpLabel = document.getElementById('#cSharpLabel');


var featuredBlog = JSON.parse(document.querySelector('#featuredBlog').getAttribute("value"));
var blogsData = document.querySelectorAll('div.blogsData');
var blogCategoryData = JSON.parse(document.querySelector('#blogCategoryData').getAttribute("value"));

let blogsToShow = [];


const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];




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
    }
}


const getBlogsFromSearch = (searchString) => {
    console.log("search string: " + searchString);
    blogsToShow = [];

    blogsData.forEach(b => {
        var blog = JSON.parse(b.getAttribute("value"));
        
        if (blog.title.toString().toLowerCase().includes(searchString) ||
            blog.description.toString().toLowerCase().includes(searchString) ||
            blog.content.toString().toLowerCase().includes(searchString) || 
            searchString.toString() === "") {

            console.log(blog);
            blogsToShow.push(blog);
            console.log(blogsToShow[0]);
        }
    });
    displayBlogs();
}

const getBlogsFromSelectedCategory = (catName) => {
    console.log("clicked selected cat: " + catName);
    blogsToShow = [];

    blogsData.forEach(b => {
        var blog = JSON.parse(b.getAttribute("value"));
        var cat = getBlogCategoryName(blog.category);

        if (cat == catName) {

            blogsToShow.push(blog);
            console.log(blog);
        }
    });

    displayBlogs();
}

const displayFeaturedBlog = () => {
    //featuredBlogContainer.innerHTML = '';
    console.log('display featured blog: ' + featuredBlog);

    var createdOnDate = new Date(featuredBlog.createdOn);
    var year = createdOnDate.getFullYear();
    var month = months[createdOnDate.getMonth()].substring(0, 3);
    var date = createdOnDate.getDate();
    var date = createdOnDate.getDate();

    const divElement = document.createElement('div');
    divElement.innerHTML =
        `<div class="card mb-4">
                <a href="/Home/Details/${featuredBlog.id}">
                    <img class="card-img-top" src="https://dummyimage.com/850x350/dee2e6/6c757d.jpg" alt="..." />
                </a>
                <div class="card-body">
                    <div class="small text-muted">${month} ${date}, ${year}</div>
                    <h2 class="card-title h4"><a href="/Blog/Details/${featuredBlog.id}">${featuredBlog.title}</a></h2>
                    <p class="card-text">${featuredBlog.description}</p>
                </div>
            </div>`;

    featuredBlogContainer.appendChild(divElement);
}

const displayBlogs = () => {
    blogsDisplayContainer.innerHTML = '';
    console.log('display blogs: ' + blogsToShow);

    for (var i = 0; i < blogsToShow.length; i++) {

        if (blogsToShow[i].isFeatured) return;

        var createdOnDate = new Date(blogsToShow[i].createdOn);
        var year = createdOnDate.getFullYear();
        var month = months[createdOnDate.getMonth()].substring(0, 3);
        var date = createdOnDate.getDate();
        var date = createdOnDate.getDate();

        const divElement = document.createElement('div');
        divElement.innerHTML =
           `<div class="col-lg-6">
                <div class="card mb-4">
                    <a href="/Home/Details/${blogsToShow[i].id}">
                        <img class="card-img-top" src="https://dummyimage.com/700x350/dee2e6/6c757d.jpg" alt="..." />
                    </a>
                    <div class="card-body">
                        <div class="small text-muted">${month} ${date}, ${year}</div>
                        <h2 class="card-title h4"><a href="/Home/Details/${blogsToShow[i].id}">${blogsToShow[i].title}</a></h2>
                        <p class="card-text">${blogsToShow[i].description}</p>
                    </div>
                </div>
            </div>`;

        blogsDisplayContainer.prepend(divElement);
    }
}


const setUpBlogCategoryList = () => {
    blogCategoryData.forEach(cat => {
        var catName = getBlogCategoryName(cat);

        const listItemElement = document.createElement('li');
        listItemElement.innerHTML = `<a href="#" onclick="return false;">${catName}</a>`;
        listItemElement.classList = 'list-select';
        listItemElement.addEventListener('click', () => getBlogsFromSelectedCategory(catName));

        blogCategoryListContainer.append(listItemElement);
    });
}

searchBarInput.addEventListener('input', (e) => {
    getBlogsFromSearch(e.target.value);
})


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

setUpBlogCategoryList();
getBlogsFromSearch("");
displayFeaturedBlog();