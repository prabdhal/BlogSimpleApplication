let postData = document.querySelector('div.blogData');
const categoryBadgeContainer = document.querySelector('#categoryBadgeContainer');
const postCategoryListContainer = document.querySelector('#blogCategoryListContainer');

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
    let replyForm = el.parentElement.parentElement.parentElement.parentElement.nextElementSibling.nextElementSibling;

    replyForm.style.display = 'flex';
}

const hideCreateReplyForm = (el) => {
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
}

//  displays the edit comment container
const displayEditInputForComment = (el) => {
    let commentContainer = el.parentElement.parentElement.parentElement.parentElement.parentElement;
    let editCommentContainer = commentContainer.nextElementSibling;
    hideAllEditCommentInputs();

    commentContainer.style.display = 'none';
    editCommentContainer.style.display = 'block';
}

//  displays the edit replycontainer
const displayEditInputForReply = (el) => {
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
    deleteCommentModalForm.action = `/Post/DeleteComment/${commentId}`;
}

// displays delete reply modal
const displayDeleteReplyModal = (replyId) => {
    deleteReplyModal.style.display = 'block';
    deleteReplyModalForm.action = `/Post/DeleteReply/${replyId}`;
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
    let commentFormBtns = el.nextElementSibling;

    commentFormBtns.style.display = 'flex';
}

// hides comment section buttons 
const hideCommentButtons = (el) => {
    let commentForm = el.parentElement;
    let commentFormTextArea = el.parentElement.previousElementSibling;

    commentFormTextArea.value = '';
    commentForm.style.display = 'none';
}

if (commentBtn != null) {
    commentBtn.addEventListener('click', (e) => commentButtonValidationCheck(e));
}

const commentButtonValidationCheck = (e) => {

    if (commentTextarea.value == null || commentTextarea.value == '') {
        e.preventDefault();
        alert("value: " + commentTextarea.value + " is empty!");
    }
}

// create category badge on detail view dashboard
const createCategoryBadge = () => {
    let post = JSON.parse(postData.getAttribute("value"));

    const badgeElement = document.createElement('a');
    badgeElement.innerHTML = `<a class="badge text-decoration-none link-light ${getPostCategoryClass(post.category)}">${getPostCategoryName(post.category)}</a>`;

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
const getPostCategoryName = (value) => {
    switch (value) {
        case 0:
            return 'Programming';
        case 1:
            return 'Game Development';
        case 2:
            return 'Web Development';
        case 3:
            return 'General Tutorials';
        case 4:
            return 'Other';
        default:
            return 'All';
    }
}

// maps enum name into ints
const getPostCategoryIdx = (value) => {
    switch (value) {
        case 'Programming':
            return 0;
        case 'Game Development':
            return 1;
        case 'Web Development':
            return 2;
        case 'General Tutorials':
            return 3;
        case 'Other':
            return 4;
        default:
            return 100;
    }
}

// maps enum int to color
const getPostCategoryClass = (value) => {
    switch (value) {
        case 0:
            return 'programming';
        case 1:
            return 'game-development';
        case 2:
            return 'web-development';
        case 3:
            return 'general-tutorials';
        case 4:
            return 'other';
        default:
            return 'all';
    }
}

createCategoryBadge();