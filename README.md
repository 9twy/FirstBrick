# FirstBrick - Real Estate Crowdfunding Platform  

## Overview  

FirstBrick is a modern real estate crowdfunding platform designed to make property investment simple and accessible for everyone. Built with scalability in mind, the platform connects investors with real estate projects, offering transparency and ease of use.  

Unlike traditional investment platforms, FirstBrick provides:  
- **User-friendly investment tracking** – Monitor your portfolio in real time.  
- **Seamless crowdfunding** – Invest in projects with just a few clicks.  
- **Secure transactions** – Fast and reliable payment processing.  

## Technical Stack  

- **Backend**: .NET Core (Microservices Architecture)  
- **Messaging**: RabbitMQ (for inter-service communication)  
- **Authentication**: JWT (Secure access control)  

## Core Microservices  

### 1. **Account Service**  
   - User registration & authentication  
   - Profile management  

### 2. **Investment Service**  
   - Real estate project listings  
   - Investment processing  

### 3. **Portfolio Service**  
   - Track investments & performance  
   - Detailed project insights  

### 4. **Payment Service**  
   - Balance management  
   - Apple Pay integration (mock)  
   - Transaction history  

## API Endpoints  

### **Account Service**  
- `POST /v1/user` – Register a new user  
- `POST /v1/login` – Authenticate & retrieve JWT  
- `GET /v1/user/{user_id}` – Fetch user profile  
- `PUT /v1/user/{user_id}` – Update profile  

### **Investment Service**  
- `POST /v1/project` – Create a new project  
- `GET /v1/projects` – List all projects  
- `POST /v1/invest` – Invest in a project  

### **Portfolio Service**  
- `GET /v1/portfolio` – View user portfolio  
- `GET /v1/portfolio/{project_id}` – Project details  

### **Payment Service**  
- `POST /v1/ApplepayTopup` – Mock Apple Pay top-up  
- `GET /v1/balance` – Check user balance  
- `GET /v1/transactions` – View transaction history  

## Development Phases  

### **Phase 1: Foundation**  
- Design database schema  
- Establish service communication (RabbitMQ)  

### **Phase 2: Implementation**  
- Develop all API endpoints  
- Secure services with JWT authentication  
- Define RabbitMQ event flows  

