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
    - [ ] Adding schedule event record (for checking API avability)
    - [ ] Display all schedule events assigned to account
    - [ ] Function for connecting mobile device with schedule event results

### Serverless [Azure functions](https://azure.microsoft.com/pl-pl/updates/announcing-go-live-release-for-azure-functions-v3)
- [ ] Function for checking API avability

### Database [CosmosDB](https://azure.microsoft.com/pl-pl/services/cosmos-db/)
https://cerber-db.documents.azure.com:443/