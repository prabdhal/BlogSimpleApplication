let blogData = document.querySelector('div.blogData');
const categoryBadgeContainer = document.querySelector('#categoryBadgeContainer');


let dropdownContents = document.querySelectorAll('.dropdown-content');
const commentTextarea = document.querySelector('.comment-textarea'); 
const createCommentButtons = document.querySelector('.create-comment-buttons'); 
const hideCommentButtonsBtn = document.querySelector('#hideCommentButtons'); 
const commentBtn = document.querySelector('#commentBtn'); 

const commentDisplaySections = document.querySelectorAll('.comment-display-section');
const commentEditDisplaySections = document.querySelectorAll('.comment-edit-display-section');

const replyDisplaySections = document.querySelectorAll('.reply-display-section');
const replyEditDisplaySections = document.querySelectorAll('.reply-edit-display-section');

const deleteCommentModal = document.querySelector('.delete-comment-modal');
const deleteCommentModalForm = document.querySelector('#deleteCommentModalForm');

const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

// displays reply form 
const displayCreateReplyForm = (el) => {
    console.log(el.parentElement.parentElement.parentElement.parentElement.nextElementSibling.nextElementSibling);
    let replyForm = el.parentElement.parentElement.parentElement.parentElement.nextElementSibling.nextElementSibling;

    replyForm.style.display = 'flex';
}

const hideCreateReplyForm = (el) => {
    console.log(el.parentElement.parentElement.parentElement);
    let replyForm = el.parentElement.parentElement.parentElement;

    replyForm.style.display = 'none';
}

// opens the comment drop down menu
const openCommentDropDownMenuContent = (el) => {
    if (el.nextElementSibling.style.display == 'block') {
        closeAllCommentDropDownModal();
    } else {
        el.nextElementSibling.style.display = 'block';
    }
    console.log('openCommentDropDownMenu');
}

//  displays the edit comment container
const displayEditInputForComment = (el) => {
    console.log(el.parentElement.parentElement.parentElement.parentElement.parentElement);
    let commentContainer = el.parentElement.parentElement.parentElement.parentElement.parentElement;
    let editCommentContainer = commentContainer.nextElementSibling;
    hideAllEditCommentInputs();

    commentContainer.style.display = 'none';
    editCommentContainer.style.display = 'block';
}

//  displays the edit replycontainer
const displayEditInputForReply = (el) => {
    console.log(el.parentElement.parentElement.parentElement.parentElement.parentElement);
    let replyContainer = el.parentElement.parentElement.parentElement.parentElement.parentElement;
    let editReplyContainer = replyContainer.nextElementSibling;
    hideAllEditCommentInputs();
    hideAllEditReplyInputs();

    replyContainer.style.display = 'none';
    editReplyContainer.style.display = 'flex';
}

// closes all comment drop down menus
const closeAllCommentDropDownModal = () => {
    dropdownContents.forEach(dropDown => {
        dropDown.style.display = 'none';
    });
}

const hideAllEditCommentInputs = () => {
    commentEditDisplaySections.forEach(editDisplay => {
        editDisplay.style.display = 'none';
    });
    displayAllComments();
    displayAllReplies();
}

const hideAllEditReplyInputs = () => {
    replyEditDisplaySections.forEach(editDisplay => {
        editDisplay.style.display = 'none';
    });
    displayAllComments();
    displayAllReplies();
}

const displayAllComments = () => {
    commentDisplaySections.forEach(commentDisplay => {
        commentDisplay.style.display = 'block';
    });
}

const displayAllReplies = () => {
    replyDisplaySections.forEach(replyDisplay => {
        replyDisplay.style.display = 'flex';
    });
}

const displayDeleteCommentModal = (commentId) => {
    deleteCommentModal.style.display = 'block';
    deleteCommentModalForm.action = `/Blog/DeleteComment/${commentId}`;
}

const hideDeleteCommentModal = () => {
    deleteCommentModal.style.display = 'none';
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

// closes menus when clicking on window
window.addEventListener('click', (e) => {
    if (e.target.classList.contains('menu-icon') == false) {
        closeAllCommentDropDownModal();
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

createCategoryBadge();