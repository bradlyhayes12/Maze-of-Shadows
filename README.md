# CS4398.Group6

# Unity and GitHub Workflow

## Cloning a Unity Project from GitHub
Pulling a Unity project from GitHub is straightforward. Just follow these steps:

1. **Clone the repository** – Use GitHub Desktop or the command line to clone the existing repo.
2. **Ensure the `.gitignore` file is included** – The `.gitignore` file in the repo is specifically configured for Unity.  
   - It excludes unnecessary files that Unity generates.  
   - This keeps the repository clean and reduces the size of pull/push operations.

## Pushing Updates to GitHub
When pushing changes to GitHub:

- Ensure that the `.gitignore` file is in the **root directory** of the Unity project.
- This prevents unwanted files from being committed and keeps the repo optimized.

## Branching Strategy
To maintain an organized workflow, we will use the following **branching system**:

- **Main Branch (`main`)** – The **"golden branch"**, used for deployment and finalized work.
- **Member Branches (Ideally 1 per team member)** – Each member works on their own branch.
  - Example: member1, member2, etc.

### How to Work with Branches
1. **Work on your own branch** – Develop features separately to avoid conflicts.
2. **Merge changes properly**  
   - Before merging into `main`, **resolve any merge conflicts**.
   - Use **pull requests (PRs)** when ready to merge.
3. **Keep `main` clean** – The `main` branch should only contain **tested, stable code**.
