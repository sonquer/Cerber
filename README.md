# Cerber
Availability check app architecture

---

## Mobile (Xamarin)
- [x] **Cerber**
    - [x] Login page
    - [x] Register page
    - [x] Display all services ( services list )
    - [x] Display status checks ( detail page )
    - [x] Create availability record
    - [x] Sqlite db - session support
    - [ ] Display account details based on JWT token
    - [ ] Logout functionality

---
## Backend
### Api (.NET Core 3.1)
- [x] **Accounts.Api**
    - [x] Add new account
    - [x] Taking jwt token based on account data
- [x] **Availability.Api**
    - [x] Adding schedule event record (for checking API avability)
    - [x] Display all schedule events assigned to account
    - [x] Function display availability details
    - [x] Function to delete availability record
    - [x] Function for update availability record

### Availability.Worker
- [x] Function for checking API avability