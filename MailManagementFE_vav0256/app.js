let state = {
    authToken: null,
    userEmail: null,
    userRole: null,
    mails: [],
    archivedMails: [],
};

const API_BASE_URL = 'http://localhost:5055';

$(document).ready(function() {
    if (state.authToken) {
        showMainApp();
    } else {
        showLoginForm();
    }
});

function showLoginForm() {
    const loginHtml = `
    <div class="container" style="margin-top: 50px;">
        <h2>Login</h2>
        <form id="login-form">
            <div class="form-group">
                <label for="email">Email:</label>
                <input type="email" class="form-control" id="email" required />
            </div>
            <div class="form-group">
                <label for="password">Password:</label>
                <input type="password" class="form-control" id="password" required />
            </div>
            <button type="submit" class="btn btn-primary">Login</button>
            <div id="login-error" class="text-danger mt-2" style="display: none;">Invalid email or password.</div>
        </form>
    </div>
    `;
    $('#app').html(loginHtml);

    $('#login-form').submit(function(e) {
        e.preventDefault();
        const email = $('#email').val();
        const password = $('#password').val();
        login(email, password);
    });
}

function login(email, password) {
    $.ajax({
        url: `${API_BASE_URL}/User/login`,
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ email: email, password: password }),
        success: function(data) {
            state.authToken = data;
            const authData = atob(state.authToken);
            const parts = authData.split(':');
            state.userEmail = parts[0];
            state.userRole = parts[1];

            showMainApp();
        },
        error: function(jqXHR) {
            if (jqXHR.status === 401) {
                $('#login-error').show();
            } else {
                alert('An error occurred during login.');
            }
        }
    });
}

function showMainApp() {
    let menuItems = `
        <li class="list-group-item"><a href="#" id="nav-mails">Mails</a></li>
        <li class="list-group-item"><a href="#" id="nav-archive">Archive</a></li>
    `;
    if (state.userRole === 'Receptionist' || state.userRole === 'Administrator') {
        menuItems += `
            <li class="list-group-item"><a href="#" id="nav-statistics">Statistics</a></li>
        `;
    }
    if (state.userRole === 'Administrator') {
        menuItems += `
            <li class="list-group-item"><a href="#" id="nav-admin">Admin</a></li>
        `;
    }
    const mainAppHtml = `
    <div class="main-container">
        <!-- Sidebar -->
        <div id="sidebar">
            <ul class="list-group list-group-flush">
                ${menuItems}
            </ul>
            <div class="mt-auto p-3">
                <p>${state.userEmail}</p>
                <p>${state.userRole}</p>
                <button class="btn btn-secondary btn-sm" id="logout-button">Log Out</button>
            </div>
        </div>
        <div id="content-area">
        </div>
    </div>
    `;

    $('#app').html(mainAppHtml);

    $('#nav-mails').click(function(e) {
        e.preventDefault();
        showMailsPage();
    });

    $('#nav-archive').click(function(e) {
        e.preventDefault();
        showArchivePage();
        $('#add-mail-button').remove();
    });

    if (state.userRole === 'Receptionist' || state.userRole === 'Administrator') {
        $('#nav-statistics').click(function(e) {
            e.preventDefault();
            showStatisticsPage();
            $('#add-mail-button').remove();
        });
    }

    if (state.userRole === 'Administrator') {
        $('#nav-admin').click(function(e) {
            e.preventDefault();
            showAdminPage();
            $('#add-mail-button').remove();
        });
    }

    $('#logout-button').click(function() {
        logout();
    });

    // Default to Mails page
    showMailsPage();
}

function logout() {
    state.authToken = null;
    state.userEmail = null;
    state.userRole = null;
    showLoginForm();
}

function showMailsPage() {
    const mailsPageHtml = `
        <h2>Mails</h2>
        <input type="text" id="search-input" class="form-control mb-3" placeholder="Search...">
        <div id="mails-list"></div>
    `;
    $('#content-area').html(mailsPageHtml);

    loadMails();

    if (state.userRole === 'Receptionist' || state.userRole === 'Administrator') {
        const addButtonHtml = `
        <button id="add-mail-button" class="btn btn-primary" style="position: fixed; bottom: 20px; right: 20px; border-radius: 50%; width: 50px; height: 50px; font-size: 24px;">+</button>
        `;
        $('#app').append(addButtonHtml);

        $('#add-mail-button').click(function() {
            showAddMailDialog();
        });
    } else {
        $('#add-mail-button').remove();
    }
}

function loadMails() {
    $.ajax({
        url: `${API_BASE_URL}/Mail`,
        method: 'GET',
        headers: {
            'Authorization': state.authToken
        },
        success: function(data) {
            state.mails = data; // Store the data for searching
            displayMails(data, true, '#mails-list');

            $('#search-input').on('input', function() {
                const query = $(this).val().toLowerCase();
                const filteredData = state.mails.filter(mail => {
                    return JSON.stringify(mail).toLowerCase().includes(query);
                });
                displayMails(filteredData, true, '#mails-list');
            });
        },
        error: function() {
            alert('Failed to load mails.');
        }
    });
}

function displayMails(mails, isMailsPage, containerId = '#mails-list') {
    let mailsHtml = '<table class="table table-striped"><thead><tr>';
    mailsHtml += '<th>ID</th>';
    mailsHtml += '<th>Mail Type</th>';
    mailsHtml += '<th>Description</th>';
    mailsHtml += '<th>Sender</th>';
    mailsHtml += '<th>Recipient</th>';
    mailsHtml += '<th>Receptionist</th>';
    mailsHtml += '<th>Status</th>';
    mailsHtml += '<th>Received Date</th>';

    if (isMailsPage && (state.userRole === 'Receptionist' || state.userRole === 'Administrator')) {
        mailsHtml += '<th>Action</th>';
    } else if (!isMailsPage) {
        mailsHtml += '<th>Claimed Date</th>';
    }

    mailsHtml += '</tr></thead><tbody>';

    mails.forEach(function(mail) {
        mailsHtml += '<tr>';
        mailsHtml += `<td>${mail.id}</td>`;
        mailsHtml += `<td>${mail.mailType}</td>`;
        mailsHtml += `<td>${mail.description}</td>`;
        mailsHtml += `<td>${mail.sender ? mail.sender.name : ''}</td>`;
        mailsHtml += `<td>${mail.recipient ? mail.recipient.email : ''}</td>`;
        mailsHtml += `<td>${mail.receptionist ? mail.receptionist.email : ''}</td>`;
        mailsHtml += `<td>${mail.status}</td>`;
        mailsHtml += `<td>${mail.receivedDate}</td>`;

        if (isMailsPage && (state.userRole === 'Receptionist' || state.userRole === 'Administrator')) {
            mailsHtml += `<td><button class="btn btn-success btn-sm claim-mail-button" data-mail-id="${mail.id}">Claim</button></td>`;
        } else if (!isMailsPage) {
            mailsHtml += `<td>${mail.claimedDate || ''}</td>`;
        }

        mailsHtml += '</tr>';
    });

    mailsHtml += '</tbody></table>';

    $(containerId).html(mailsHtml);

    if (isMailsPage && (state.userRole === 'Receptionist' || state.userRole === 'Administrator')) {
        $('.claim-mail-button').click(function() {
            const mailId = $(this).data('mail-id');
            claimMail(mailId);
        });
    }
}

function claimMail(mailId) {
    $.ajax({
        url: `${API_BASE_URL}/Mail/${mailId}/Claim`,
        method: 'POST',
        headers: {
            'Authorization': state.authToken
        },
        success: function() {
            loadMails();
        },
        error: function() {
            alert('Failed to claim mail.');
        }
    });
}

function showArchivePage() {
    const archivePageHtml = `
        <h2>Archive</h2>
        <input type="text" id="search-input" class="form-control mb-3" placeholder="Search...">
        <div id="archive-list"></div>
    `;
    $('#content-area').html(archivePageHtml);

    loadArchive();

    $('#add-mail-button').remove();
}

function loadArchive() {
    $.ajax({
        url: `${API_BASE_URL}/Mail/archive`,
        method: 'GET',
        headers: {
            'Authorization': state.authToken
        },
        success: function(data) {
            state.archivedMails = data; // Store data for searching
            displayMails(data, false, '#archive-list');

            $('#search-input').on('input', function() {
                const query = $(this).val().toLowerCase();
                const filteredData = state.archivedMails.filter(mail => {
                    return JSON.stringify(mail).toLowerCase().includes(query);
                });
                displayMails(filteredData, false, '#archive-list');
            });
        },
        error: function() {
            alert('Failed to load archive.');
        }
    });
}

function showAddMailDialog() {
    $.when(fetchSenders(), fetchUsers()).done(function(sendersData, usersData) {
        const senders = sendersData[0];
        const users = usersData[0];

        let senderOptions = '';
        senders.forEach(function(sender) {
            senderOptions += `<option value="${sender.id}">${sender.name}</option>`;
        });

        let recipientOptions = '';
        users.forEach(function(user) {
            recipientOptions += `<option value="${user.id}">${user.email}</option>`;
        });

        const dialogHtml = `
        <div class="modal" tabindex="-1" role="dialog" id="add-mail-modal">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <form id="add-mail-form">
              <div class="modal-header">
                <h5 class="modal-title">Add New Mail</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="close-add-mail-modal">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                    <div class="form-group">
                        <label for="mailType">Mail Type</label>
                        <input type="text" class="form-control" id="mailType" required />
                    </div>
                    <div class="form-group">
                        <label for="description">Description</label>
                        <textarea class="form-control" id="description"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="recipientId">Recipient</label>
                        <select class="form-control" id="recipientId" required>
                            <option value="">Select Recipient</option>
                            ${recipientOptions}
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="senderId">Sender</label>
                        <select class="form-control" id="senderId" required>
                            <option value="">Select Sender</option>
                            ${senderOptions}
                        </select>
                    </div>
              </div>
              <div class="modal-footer">
                <button type="submit" class="btn btn-primary">Add Mail</button>
                <button type="button" class="btn btn-secondary" data-dismiss="modal" id="cancel-add-mail-modal">Cancel</button>
              </div>
              </form>
            </div>
          </div>
        </div>
        `;

        $('#app').append(dialogHtml);

        $('#add-mail-modal').modal('show');

        $('#add-mail-form').submit(function(e) {
            e.preventDefault();
            const mailData = {
                mailType: $('#mailType').val(),
                description: $('#description').val(),
                recipientId: $('#recipientId').val(),
                senderId: $('#senderId').val(),
            };
            addMail(mailData);
        });

        $('#close-add-mail-modal').click(function() {
            $('#add-mail-modal').modal('hide').remove();
        });

        $('#cancel-add-mail-modal').click(function() {
            $('#add-mail-modal').modal('hide').remove();
        });

    }).fail(function() {
        alert('Failed to load senders or users.');
    });
}

function fetchSenders() {
    return $.ajax({
        url: `${API_BASE_URL}/Sender`,
        method: 'GET',
        headers: {
            'Authorization': state.authToken
        }
    });
}

function fetchUsers() {
    return $.ajax({
        url: `${API_BASE_URL}/User`,
        method: 'GET',
        headers: {
            'Authorization': state.authToken
        }
    });
}

function addMail(mailData) {
    $.ajax({
        url: `${API_BASE_URL}/Mail`,
        method: 'POST',
        contentType: 'application/json',
        headers: {
            'Authorization': state.authToken
        },
        data: JSON.stringify(mailData),
        success: function(data) {
            $('#add-mail-modal').modal('hide').remove();
            loadMails();
        },
        error: function() {
            alert('Failed to add mail.');
        }
    });
}

function showStatisticsPage() {
    $('#content-area').html(`
        <h2>Statistics for the last 30 days</h2>
        <div id="stats-summary" class="mb-4"></div>
        <canvas id="stats-chart" width="400" height="200"></canvas>
    `);

    fetchStatisticsData();
}

function fetchStatisticsData() {
$.ajax({
        url: `${API_BASE_URL}/Mail`,
        method: 'GET',
        headers: {
            'Authorization': state.authToken
        },
        success: function(mails) {
            processStatisticsData(mails);
        },
        error: function() {
            alert('Failed to load statistics data.');
        }
    });
}

function processStatisticsData(mails) {
    const now = new Date();
    const thirtyDaysAgo = new Date();
    thirtyDaysAgo.setDate(now.getDate() - 29);

    const dates = [];
    const receivedCounts = [];
    const claimedCounts = [];

    for (let i = 0; i < 30; i++) {
        const date = new Date(thirtyDaysAgo);
        date.setDate(thirtyDaysAgo.getDate() + i);
        const dateString = date.toISOString().split('T')[0];
        dates.push(dateString);
        receivedCounts.push(0);
        claimedCounts.push(0);
    }

    const dateIndexMap = {};
    dates.forEach((date, index) => {
        dateIndexMap[date] = index;
    });

    mails.forEach(mail => {
        const receivedDate = new Date(mail.receivedDate);
        const claimedDate = mail.claimedDate ? new Date(mail.claimedDate) : null;

        const receivedDateString = receivedDate.toISOString().split('T')[0];
        if (receivedDate >= thirtyDaysAgo && receivedDate <= now && dateIndexMap.hasOwnProperty(receivedDateString)) {
            receivedCounts[dateIndexMap[receivedDateString]]++;
        }

        if (claimedDate) {
            const claimedDateString = claimedDate.toISOString().split('T')[0];
            if (claimedDate >= thirtyDaysAgo && claimedDate <= now && dateIndexMap.hasOwnProperty(claimedDateString)) {
                claimedCounts[dateIndexMap[claimedDateString]]++;
            }
        }
    });

    const totalReceived = receivedCounts.reduce((a, b) => a + b, 0);
    const totalClaimed = claimedCounts.reduce((a, b) => a + b, 0);

    $('#stats-summary').html(`
        <p>Total mails received in last 30 days: <strong>${totalReceived}</strong></p>
        <p>Total mails claimed in last 30 days: <strong>${totalClaimed}</strong></p>
    `);

    renderStatisticsChart(dates, receivedCounts, claimedCounts);
}

function renderStatisticsChart(dates, receivedCounts, claimedCounts) {
    const ctx = document.getElementById('stats-chart').getContext('2d');

    const data = {
        labels: dates,
        datasets: [
            {
                label: 'Received Mails',
                data: receivedCounts,
                borderColor: 'blue',
                backgroundColor: 'rgba(0, 0, 255, 0.1)',
                fill: true,
            },
            {
                label: 'Claimed Mails',
                data: claimedCounts,
                borderColor: 'green',
                backgroundColor: 'rgba(0, 255, 0, 0.1)',
                fill: true,
            },
        ]
    };

    const chart = new Chart(ctx, {
        type: 'line',
        data: data,
        options: {
            responsive: true,
            scales: {
                x: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Date'
                    },
                    ticks: {
                        maxTicksLimit: 10,
                        callback: function(value, index, values) {
                            // Show date labels at intervals
                            return dates[index];
                        }
                    }
                },
                y: {
                    display: true,
                    title: {
                        display: true,
                        text: 'Number of Mails'
                    },
                    beginAtZero: true,
                    ticks: {
                        precision: 0
                    }
                }
            }
        }
    });
}

function showAdminPage() {
    const adminPageHtml = `
        <h2>Admin</h2>
        <div class="admin-users-section mb-4">
            <h4>Manage Users</h4>
            <div class="form-group">
                <label for="user-select">Select User:</label>
                <select id="user-select" class="form-control">
                    <option value="">--Select User--</option>
                </select>
            </div>
            <div class="form-group">
                <label for="user-role">Set Role:</label>
                <select id="user-role" class="form-control">
                    <option value="User">User</option>
                    <option value="Receptionist">Receptionist</option>
                    <option value="Administrator">Administrator</option>
                </select>
            </div>
            <button class="btn btn-primary" id="update-user-role">Update Role</button>
        </div>
        <hr>
        <div class="admin-senders-section">
            <h4>Manage Senders</h4>
            <button class="btn btn-success mb-3" id="add-sender-button">Add New Sender</button>
            <div id="senders-list"></div>
        </div>
    `;
    $('#content-area').html(adminPageHtml);

    loadUsersForAdmin();
    loadSendersForAdmin();

    $('#update-user-role').click(function() {
        const userId = $('#user-select').val();
        const newRole = $('#user-role').val();
        if (userId) {
            updateUserRole(userId, newRole);
        } else {
            alert('Please select a user.');
        }
    });

    $('#add-sender-button').click(function() {
        showAddSenderDialog();
    });
}

function loadUsersForAdmin() {
    fetchUsers().done(function(users) {
        let userOptions = '';
        users.forEach(function(user) {
            userOptions += `<option value="${user.id}">${user.email}</option>`;
        });
        $('#user-select').append(userOptions);
    }).fail(function() {
        alert('Failed to load users.');
    });
}

function updateUserRole(userId, newRole) {
    $.ajax({
        url: `${API_BASE_URL}/User/${userId}`,
        method: 'PUT',
        contentType: 'application/json',
        headers: {
            'Authorization': state.authToken
        },
        data: JSON.stringify({ role: newRole }),
        success: function() {
            alert('User role updated successfully.');
        },
        error: function() {
            alert('Failed to update user role.');
        }
    });
}

function loadSendersForAdmin() {
    fetchSenders().done(function(senders) {
        displaySenders(senders);
    }).fail(function() {
        alert('Failed to load senders.');
    });
}

function displaySenders(senders) {
    let sendersHtml = '<table class="table table-striped"><thead><tr>';
    sendersHtml += '<th>ID</th>';
    sendersHtml += '<th>Name</th>';
    sendersHtml += '<th>Contact Info</th>';
    sendersHtml += '</tr></thead><tbody>';

    senders.forEach(function(sender) {
        sendersHtml += '<tr>';
        sendersHtml += `<td>${sender.id}</td>`;
        sendersHtml += `<td>${sender.name}</td>`;
        sendersHtml += `<td>${sender.contactInfo}</td>`;
        sendersHtml += '</tr>';
    });

    sendersHtml += '</tbody></table>';

    $('#senders-list').html(sendersHtml);
}

function showAddSenderDialog() {
    const dialogHtml = `
    <div class="modal" tabindex="-1" role="dialog" id="add-sender-modal">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <form id="add-sender-form">
          <div class="modal-header">
            <h5 class="modal-title">Add New Sender</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="close-add-sender-modal">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
                <div class="form-group">
                    <label for="sender-name">Name</label>
                    <input type="text" class="form-control" id="sender-name" required />
                </div>
                <div class="form-group">
                    <label for="sender-contact-info">Contact Info</label>
                    <input type="text" class="form-control" id="sender-contact-info" />
                </div>
          </div>
          <div class="modal-footer">
            <button type="submit" class="btn btn-primary">Add Sender</button>
            <button type="button" class="btn btn-secondary" data-dismiss="modal" id="cancel-add-sender-modal">Cancel</button>
          </div>
          </form>
        </div>
      </div>
    </div>
    `;

    $('#app').append(dialogHtml);

    $('#add-sender-modal').modal('show');

    $('#add-sender-form').submit(function(e) {
        e.preventDefault();
        const senderData = {
            name: $('#sender-name').val(),
            contactInfo: $('#sender-contact-info').val()
        };
        addSender(senderData);
    });

    $('#close-add-sender-modal').click(function() {
        $('#add-sender-modal').modal('hide').remove();
    });

    $('#cancel-add-sender-modal').click(function() {
        $('#add-sender-modal').modal('hide').remove();
    });
}

function addSender(senderData) {
    $.ajax({
        url: `${API_BASE_URL}/Sender`,
        method: 'POST',
        contentType: 'application/json',
        headers: {
            'Authorization': state.authToken
        },
        data: JSON.stringify(senderData),
        success: function() {
            $('#add-sender-modal').modal('hide').remove();
            loadSendersForAdmin();
        },
        error: function() {
            alert('Failed to add sender.');
        }
    });
}
