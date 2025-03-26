class UI {
    get btnDeleteCustomers() { return document.querySelectorAll(".btnDeleteCustomer") }
    get btnUpdateCustomers() { return document.querySelectorAll(".btnUpdateCustomer") }
    get btnUpdateCustomerSaves() { return document.querySelectorAll(".btnUpdateCustomerSave") }
    get btnUpdateCustomerCancels() { return document.querySelectorAll(".btnUpdateCustomerCancel") }
    get txtCustomerFirstName() { return document.getElementById("FirstName") }
    get lblCustomerCount() { return document.getElementById("lblCustomerCount") }
    get lblUserCount() { return document.getElementById("lblUserCount") }
    get tblCustomers() { return document.getElementById("tblCustomers") }
}

var customerDatatable;
const customerTableState = Object.freeze({
    update: "update",
    cancel: "cancel",
    save: "save",
    delete: "delete"
});

const ui = new UI();

document.addEventListener("DOMContentLoaded", () => {
    if (ui.tblCustomers) {
        customerDatatable = new DataTable(ui.tblCustomers, {
            "language": {
                "search": "Filter records:"
            }
        });

        $('input[type = search]').on('keyup', function() {
            customerDatatable.search('^' + this.value, true, false).draw();
        });
    }
    ui.txtCustomerFirstName?.focus();

    for (const btn of ui.btnDeleteCustomers) {
        btn.addEventListener("click", (e) => {

            const id = btn.dataset.id;
            const name = btn.dataset.name;

            Swal.fire({
                title: "Are you sure?",
                html: `Do you really want to delete the customer named <b>${name}</b>?`,
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes",
                cancelButtonText: "No"
            }).then(async (result) => {
                if (result.isConfirmed) {
                    const response = await fetch("/customer/delete?id=" + id);
                    const responseCustomer = await response.json();
                    if (responseCustomer) {
                        toastr["success"](`${name} success deleted`);
                        const trToRemove = document.querySelector(`[data-id='${id}']`)?.closest("tr");
                        $(trToRemove).fadeOut("slow");
                        reloadDashboardHeader("customer");
                    }
                }
            });
        });
    }

    for (const btn of ui.btnUpdateCustomers) {
        btn.addEventListener("click", async (e) => {
            const updateRow = btn.closest("tr");
            refreshCustomerRow(updateRow, customerTableState.update);
        });
    }

    for (const btn of ui.btnUpdateCustomerCancels) {
        btn.addEventListener("click", async (e) => {
            const updateRow = btn.closest("tr");
            refreshCustomerRow(updateRow, customerTableState.cancel);
        });
    }

    for (const btn of ui.btnUpdateCustomerSaves) {
        btn.addEventListener("click", async (e) => {
            const updateRow = btn.closest("tr");
            const inputs = updateRow.querySelectorAll("input")


            const id = +inputs[0].value;

            const firstName = inputs[1].value;
            const lastName = inputs[2].value;
            const email = inputs[3].value;
            const registrationDate = inputs[4].value;
            const select = updateRow.querySelector("select");
            const region = select.options[select.selectedIndex].value;

            const customerModel = new URLSearchParams({ id, firstName, lastName, email, region, registrationDate });

            const response = await fetch("/customer/update?" + customerModel);
            if (response.ok && response.status == 200) {
                const responseCustomer = await response.json();
                if (responseCustomer) {
                    const labels = updateRow.querySelectorAll("label");
                    labels[0].innerText = `${firstName} ${lastName}`;
                    labels[1].innerText = email;
                    labels[2].innerText = region;
                    const regAsDate = new Date(registrationDate)
                    const month = regAsDate.toLocaleString("en-US", { month: "long" });

                    labels[3].innerText = `${regAsDate.getDate()} ${month} ${regAsDate.getFullYear()}`;

                    refreshCustomerRow(updateRow, customerTableState.save);
                    toastr["success"](`${firstName} ${lastName} update success`);
                }
                else {
                    toastr["error"](`${firstName} ${lastName} update failed`);
                }
            }
            else {
                toastr["warning"](`${firstName} ${lastName} update failed.You have to change the values!`);
            }
        });
    }

    if ($("#slcCountries").length) {
        $("#slcCountries").select2();
    }
});

const refreshCustomerRow = (updateRow, customerTableState) => {
    const rowColumns = $(updateRow).children("td");
    switch (customerTableState) {
        case "update":
            rowColumns.children("[data-state='cancel']").addClass("d-none");
            rowColumns.children("[data-state='update']").removeClass("d-none");
            updateRow.querySelector("input").focus();
            break;
        case "save":
        case "cancel":
            rowColumns.children("[data-state='cancel']").removeClass("d-none");
            rowColumns.children("[data-state='update']").addClass("d-none");
            break;
        case "delete":
            break;
        default:
    }
}

const showElement = (element, parent) => {
    parent ??= document;
    parent.querySelector(`#${element.id}`).classList.remove("d-none");
    parent.querySelector(`#${element.id}`).classList.add("d-inline");
}

const hideElement = (element, parent) => {
    parent ??= document;
    parent.querySelector(`#${element.id}`).classList.remove("d-inline");
    parent.querySelector(`#${element.id}`).classList.add("d-none");
}

const hideElements = (elements) => {
    for (const element in elements) {
        hideElement(element);
    }
}

const showElements = (elements) => {
    for (const element in elements) {
        showElement(element);
    }
}

const reloadDashboardHeader = (entityType) => {
    let customerCount = +ui.lblCustomerCount.innerText;
    let userCount = +ui.lblUserCount.innerText;

    if (entityType == "customer") {
        customerCount = --customerCount;
        ui.lblCustomerCount.innerText = customerCount;
    }
    else if (entityType == "user") {
        userCount = --userCount;
        ui.lblUserCount.innerText = userCount;
    }
}

toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-bottom-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}