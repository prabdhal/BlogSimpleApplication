let blogData = document.querySelector('div.blogData');
const categoryBadgeContainer = document.querySelector('#categoryBadgeContainer');


let dropdownContents = document.querySelectorAll('.dropdown-content');
const commentTextarea = document.querySelector('.comment-textarea'); 
const commentBtn = document.querySelector('#commentBtn'); 

// comment display and edit sections
const commentDisplaySections = document.querySelectorAll('.comment-display-section');
const commentEditDisplaySections = document.querySelectorAll('.comment-edit-display-section');

// reply display and edit sections
const replyDisplaySections = document.querySelectorAll('.reply-display-section');
const replyEditDisplaySections = document.querySelectorAll('.reply-edit-display-section');

// comment modals
const deleteCommentModal = document.querySelector('.delete-comment-modal');
const deleteCommentModalForm = document.querySelector('#deleteCommentModalForm');

// reply modals 
const deleteReplyModal = document.querySelector('.delete-reply-modal');
const deleteReplyModalForm = document.querySelector('#deleteReplyModalForm');

const months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];



// displays reply form 
const displayCreateReplyForm = (el) => {
    console.log(el.parentElement.parentElement.parentElement.parentElement.nextElementSibling.nextElementSibling);
    let replyForm = el.parentElement.parentElement.parentElement.parentElement.nextElementSibling.nextElementSibling;

    replyForm.style.display = 'flex';
}

const hideCreateReplyForm = (el) => {
    console.log(el.parentElement.parentElement.parentElement);
    console.log(el.parentElement.previousElementSibling);

    let replyForm = el.parentElement.parentElement.parentElement;
    let replyFormTextArea = el.parentElement.previousElementSibling;

    replyFormTextArea.value = '';
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

// hides all edit comment textarea
const hideAllEditCommentInputs = () => {
    commentEditDisplaySections.forEach(editDisplay => {
        editDisplay.style.display = 'none';
    });
    displayAllComments();
    displayAllReplies();
}

// hides all edit reply textarea
const hideAllEditReplyInputs = () => {
    replyEditDisplaySections.forEach(editDisplay => {
        editDisplay.style.display = 'none';
    });
    displayAllComments();
    displayAllReplies();
}

// displays all comments
const displayAllComments = () => {
    commentDisplaySections.forEach(commentDisplay => {
        commentDisplay.style.display = 'block';
    });
}

// displays all replies
const displayAllReplies = () => {
    replyDisplaySections.forEach(replyDisplay => {
        replyDisplay.style.display = 'flex';
    });
}

// displays delete comment modal 
const displayDeleteCommentModal = (commentId) => {
    deleteCommentModal.style.display = 'block';
    deleteCommentModalForm.action = `/Blog/DeleteComment/${commentId}`;
}

// displays delete reply modal
const displayDeleteReplyModal = (replyId) => {
    console.log(replyId);
    console.log(deleteReplyModal);
    console.log(deleteReplyModal.style.display);
    deleteReplyModal.style.display = 'block';
    deleteReplyModalForm.action = `/Blog/DeleteReply/${replyId}`;
}

// hides deletes comment modal 
const hideDeleteCommentModal = () => {
    deleteCommentModal.style.display = 'none';
}

// hides deletes reply modal 
const hideDeleteReplyModal = () => {
    deleteReplyModal.style.display = 'none';
}

// displays comment section buttons 
const displayCommentButtons = (el) => {
    console.log(el.nextElementSibling)

    let commentFormBtns = el.nextElementSibling;

    commentFormBtns.style.display = 'flex';
}

// hides comment section buttons 
const hideCommentButtons = (el) => {
    console.log(el.parentElement);

    let commentForm = el.parentElement;
    let commentFormTextArea = el.parentElement.previousElementSibling;

    commentFormTextArea.value = '';
    commentForm.style.display = 'none';
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