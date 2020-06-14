# Cerber
Availability check app architecture

---

## Mobile (Xamarin)
- [ ] **Cerber**
    - [ ] Connecting device with services
    - [ ] Display all services ( services list )
    - [ ] Display status checks ( detail page )
    - [ ] Detach service

---
## Backend
### Api (.NET Core 3.1)
- [x] **Accounts.Api**
    - [x] Add new account
    - [x] Taking jwt token based on account data
- [ ] **Availability.Api**
    - [x] Adding schedule event record (for checking API avability)
    - [x] Display all schedule events assigned to account
    - [ ] Function for connecting mobile device with schedule event results

### Availability.Worker
- [x] Function for checking API avability

### Database [CosmosDB](https://azure.microsoft.com/pl-pl/services/cosmos-db/)
https://cerber-db.documents.azure.com:443/