// account modals
const deleteAccountModal = document.querySelector('.delete-account-modal');
const deleteAccountModalForm = document.querySelector('#deleteAccountModalForm');



// opens the account drop down menu
const openAccountDropDownMenuContent = (el) => {
    if (el.nextElementSibling.style.display == 'block') {
        closeAllAccountDropDownModal();
    } else {
        el.nextElementSibling.style.display = 'block';
    }
}


// displays delete account modal 
const displayDeleteAccountModal = (userId) => {
    deleteAccountModal.style.display = 'block';
    deleteAccountModalForm.action = `/Account/DeleteUser/${userId}`;
}

// hides delete account modal 
const hideDeleteAccountModal = () => {
    deleteAccountModal.style.display = 'none';
}