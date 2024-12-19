<div style="display: flex; align-items: center; gap: 10px;">
    <img src="https://miro.medium.com/v2/resize:fit:828/format:webp/1*SLDi401dCoZ4mKakQYwxqA.png" alt="AspireApp Architecture" width="400"/>
    <img src="https://i0.wp.com/codeblog.dotsandbrackets.com/wp-content/uploads/2017/07/kubernetes.jpg?resize=821%2C714&ssl=1" alt="Kubernetes" width="240"/>
</div>
# AspireApp.AppHost

**AspireApp.AppHost** is an entry-point project for the Aspire application, designed to host and facilitate the seamless integration of the API service and web components. Built on modern .NET 8.0 technologies, it provides robust functionality for hosting services in a scalable and efficient way.

---

## Features

- **.NET 8.0 Support**: Leverages the latest features and performance enhancements of .NET 8.0.
- **Multi-Project Integration**:
  - Includes and references:
    - **AspireApp.ApiService** for backend API services.
    - **AspireApp.Web** for web-based user interfaces.
---

## Project Structure

This repository includes the following key parts:

1. **AspireApp.AppHost**:
   - A console application (output type `Exe`) used to host all services in a unified environment.

2. **AspireApp.ApiService**:
   - Contains backend logic and API endpoints to handle application data and interactions.

3. **AspireApp.Web**:
   - Implements the web interface for users to interact with the backend services.

4. **Dependencies**:
   - The project relies on the `Aspire.Hosting.AppHost` package (v8.2.2), which simplifies the hosting and dependency management within the .NET ecosystem.

---

## Getting Started

To deploy **AspireApp.AppHost** in a **Kubernetes** environment using **Aspirate**, follow these steps:

### Prerequisites
1. **Install Docker Desktop**  
   Ensure Docker Desktop is installed and running on your system. If not, download and install it from the [Docker Desktop website](https://www.docker.com/products/docker-desktop).  

2. **Enable Kubernetes in Docker Desktop**  
   Start Kubernetes on Docker Desktop following the official Docker documentation.

3. **Install `aspirate` CLI Tool**  
   Use the following command to install the `aspirate` tool globally [aspirate](https://www.nuget.org/packages/aspirate/):
   ```bash
   dotnet tool install --global aspirate --version 8.0.7
   ```

---

### Deployment Instructions

1. **Apply Aspirate Configuration**  
   After setting up, navigate to the directory containing the generated Aspirate output and apply it using the following command:
   ```bash
   aspirate apply
   ```

2. **Port Forwarding to Access Aspire Dashboard**  
   Forward the port to access the Aspire dashboard with the following command:
   ```bash
   kubectl port-forward service/aspire-dashboard 18888:18888
   ```
   You can now access the Aspire dashboard at `http://localhost:18888`.

3. **Port Forwarding to Access Web Frontend**  
   Forward the ports for accessing the frontend of your web application:
   ```bash
   kubectl port-forward service/webfrontend 8080:8080 8443:8443
   ```
   - Access the web frontend at `http://localhost:8080` or `https://localhost:8443`.

---






