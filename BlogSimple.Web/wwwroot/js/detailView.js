let blogData = document.querySelector('div.blogData');
let commentsData = document.querySelectorAll('div.commentsData');
let repliesData = document.querySelectorAll('div.repliesData');
let userSignedInData = document.querySelector('div.userSignedIn');
let signedInUsername = document.querySelector('div.signedInUsername').getAttribute("value");

let dropdownContent = document.querySelectorAll('.dropdown-content');

const categoryBadgeContainer = document.querySelector('#categoryBadgeContainer');
const commentContainer = document.querySelector('#commentContainer');
const replyContainer = document.querySelector('#replyContainer');

const commentTextarea = document.querySelector('.comment-textarea'); 
const createCommentButtons = document.querySelector('.create-comment-buttons'); 
const hideCommentButtonsBtn = document.querySelector('#hideCommentButtons'); 
const commentBtn = document.querySelector('#commentBtn'); 


const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];


// returns comments
const getComments = () => {
    let comments = [];

    commentsData.forEach(c => {
        let comment = JSON.parse(c.getAttribute("value"));

        comments.push(comment);
    });

    //sortComments();
    return comments;
}


// returns replies
const getReplies = () => {
    let replies= [];

    repliesData.forEach(r => {
        let reply = JSON.parse(r.getAttribute("value"));

        replies.push(reply);
    });

    return replies;
}

const displayComments = () => {
    let comments = getComments();

    commentContainer.innerHTML = '';

    if (comments.length <= 0) {
        // no results
        const divElement = document.createElement('div');
        divElement.innerHTML =
            `<div>
                <p>Sorry... There are no comments at this time...</p>
            </div>`;

        commentContainer.append(divElement);
    }

    for (var i = 0; i < comments.length; i++) {
        if (comments[i] == null) {

        } else {
            //var updatedOnDate = new Date(blogsToShow[i].updatedOn);
            //var year = updatedOnDate.getFullYear();
            //var month = months[updatedOnDate.getMonth()].substring(0, 3);
            //var day = updatedOnDate.getDate();

            const divElement = document.createElement('div');
            divElement.innerHTML =
                `<!-- Comments -->
                <div class="comment-container" >
                    <div class="d-flex mb-4">
                        <!-- Parent comment-->
                        <div class="flex-shrink-0"><img class="rounded-circle" src="https://dummyimage.com/50x50/ced4da/6c757d.jpg" alt="..." /></div>
                        <div class="ms-3">
                            <div class="fw-bold">${comments[i].createdBy.userName}</div>
                            <div class="text-break">${comments[i].content}</div>
                            <div class="reply-comment-btns-container">
                                <button>Like</button>
                                <button>Reply</button>
                            </div>
                            <div class="${comments[i].id}"></div>
                            ${getReplies(comments[i])}
                        </div>
                    </div>
                    <div class="cmt"></div>            
                </div>`;

            let appendTo = divElement.querySelector('.cmt');
            displayCommentCustomizeButtons(comments[i], appendTo);
            //divElement.appendChild(commentBtn); 
            commentContainer.append(divElement);
        }
    }

    initCommentMenuBtns();
}

const displayCommentCustomizeButtons = (comment, classToAppend) => {
    let commentButtonsDisplay = document.createElement('div');
    commentButtonsDisplay.classList = 'dropdown';
    commentButtonsDisplay.innerHTML = '';

    let userSignedIn = userSignedInData.getAttribute("value");

    if (userSignedIn && signedInUsername == comment.createdBy.userName) {
        commentButtonsDisplay.innerHTML =
            `<svg class="menu-icon" width="60px" height="40px" viewBox="-2.4 -2.4 28.80 28.80" fill="none" xmlns="http://www.w3.org/2000/svg" transform="rotate(90)matrix(1, 0, 0, 1, 0, 0)"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M19 13C19.5523 13 20 12.5523 20 12C20 11.4477 19.5523 11 19 11C18.4477 11 18 11.4477 18 12C18 12.5523 18.4477 13 19 13Z" stroke="#000000" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M12 13C12.5523 13 13 12.5523 13 12C13 11.4477 12.5523 11 12 11C11.4477 11 11 11.4477 11 12C11 12.5523 11.4477 13 12 13Z" stroke="#000000" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M5 13C5.55228 13 6 12.5523 6 12C6 11.4477 5.55228 11 5 11C4.44772 11 4 11.4477 4 12C4 12.5523 4.44772 13 5 13Z" stroke="#000000" stroke-width="2.4" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>
             <div class="dropdown-content">
                <a href="/Blog/EditComment/${comment.id}">Edit</a>
                <a href="/Blog/DeleteComment/${comment.id}">Delete</a>
            <div>`
    }

    classToAppend.appendChild(commentButtonsDisplay);

    return commentButtonsDisplay;
}

const displayReplies = (comment) => {
    let replies = getReplies();

    let replyContainer = document.querySelector(`${comment.id}`);
    replyContainer.innerHTML = '';

    if (replies.length <= 0) {
        // no replies
    }

    for (var i = 0; i < replies.length; i++) {
        if (replies[i] == null) {

        } else {
            //var updatedOnDate = new Date(blogsToShow[i].updatedOn);
            //var year = updatedOnDate.getFullYear();
            //var month = months[updatedOnDate.getMonth()].substring(0, 3);
            //var day = updatedOnDate.getDate();

            const divElement = document.createElement('div');
            divElement.innerHTML =
                `<!-- Replies -->
                <div class="d-flex mt-4">
                    <div class="flex-shrink-0"><img class="rounded-circle" src="https://dummyimage.com/50x50/ced4da/6c757d.jpg" alt="..." /></div>
                    <div class="ms-3">
                        <div class="fw-bold">${replies[i].createdBy.userName}</div>
                        <div>${replies[i].content}</div>
                    </div>
                </div>`;
        }

        replyContainer.append(divElement);
    }
}


// displays comment section buttons 
commentTextarea.addEventListener('click', () => displayCommentButtons());

const displayCommentButtons = () => {
    if (createCommentButtons.classList.contains('hide')) {
        createCommentButtons.classList.remove('hide');
    }
}

hideCommentButtonsBtn.addEventListener('click', (e) => hideCommentButtons(e));

const hideCommentButtons = (e) => {
    e.preventDefault();
    if (createCommentButtons.classList.contains('hide') == false) {
        createCommentButtons.classList.add('hide');
    }
}

commentBtn.addEventListener('click', (e) => commentButtonValidationCheck(e));

const commentButtonValidationCheck = (e) => {

    if (commentTextarea.value == null || commentTextarea.value == '') {
        e.preventDefault();
        alert("value: " + commentTextarea.value + " is empty!");
    }
}

// create category badge on detail view dashboard
const createCategoryBadge = () => {
    let blog = JSON.parse(blogData.getAttribute("value"));

    const badgeElement = document.createElement('a');
    badgeElement.innerHTML = `<a class="badge text-decoration-none link-light ${getBlogCategoryClass(blog.category)}">${getBlogCategoryName(blog.category)}</a>`;

    categoryBadgeContainer.append(badgeElement);
}

const initCommentMenuBtns = () => {
    let menuIcons = document.querySelectorAll('.menu-icon');

    // adds on click listener to all comment menus
    menuIcons.forEach(menu => {
        menu.addEventListener('click', () => {
            openCommentDropDownMenu(menu);
        });
    });
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
displayComments();