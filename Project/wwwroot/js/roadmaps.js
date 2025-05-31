document.addEventListener('DOMContentLoaded', function() {
    // Initialize theme toggle
    initThemeToggle();
    
    // Initialize roadmaps functionality
    initRoadmaps();
});

// Initialize the Theme Toggle functionality
function initThemeToggle() {
    const themeToggleBtn = document.getElementById('theme-toggle-btn');
    const htmlElement = document.documentElement;

    // Set dark mode as default
    if (!localStorage.getItem('theme')) {
        htmlElement.setAttribute('data-theme', 'dark');
        localStorage.setItem('theme', 'dark');
    } else {
        // Check for saved theme preference
        const savedTheme = localStorage.getItem('theme');
        if (savedTheme) {
            htmlElement.setAttribute('data-theme', savedTheme);
        }
    }

    // Toggle theme when button is clicked
    themeToggleBtn.addEventListener('click', function() {
        const currentTheme = htmlElement.getAttribute('data-theme');
        let newTheme;
        
        if (currentTheme === 'dark') {
            newTheme = 'light';
            htmlElement.removeAttribute('data-theme');
        } else {
            newTheme = 'dark';
            htmlElement.setAttribute('data-theme', 'dark');
        }
        
        localStorage.setItem('theme', newTheme);
        
        // Add animation effect
        document.body.classList.add('theme-transition');
        setTimeout(() => {
            document.body.classList.remove('theme-transition');
        }, 500);
    });
}

// Initialize roadmaps functionality
function initRoadmaps() {
    // Get DOM elements
    const addRoadmapBtn = document.getElementById('addRoadmapBtn');
    const addRoadmapModal = document.getElementById('addRoadmapModal');
    const notesDisplayModal = document.getElementById('notesDisplayModal');
    const closeModalBtns = document.querySelectorAll('.close-modal');
    const addRoadmapForm = document.getElementById('addRoadmapForm');
    
    // Check if user is logged in and is admin or mentor
    const currentUser = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && (currentUser.userType === 'mentor' || currentUser.role === 'admin')) {
        // Show the add roadmap button
        addRoadmapBtn.style.display = 'block';
    }
    
    // Initialize default roadmaps if none exist
    initializeDefaultRoadmaps();
    
    // Load and display roadmaps
    loadRoadmaps();
    
    // Add event listeners for the modals
    addRoadmapBtn.addEventListener('click', function() {
        addRoadmapModal.style.display = 'block';
    });
    
    // Handle closing modals with X button
    closeModalBtns.forEach(btn => {
        btn.addEventListener('click', function() {
            // Find the parent modal of this close button
            const parentModal = this.closest('.modal');
            if (parentModal) {
                parentModal.style.display = 'none';
            }
        });
    });
    
    // Close modals when clicking outside
    window.addEventListener('click', function(e) {
        if (e.target === addRoadmapModal) {
            addRoadmapModal.style.display = 'none';
        }
        if (e.target === notesDisplayModal) {
            notesDisplayModal.style.display = 'none';
        }
    });
    
    // Initialize notes display functionality
    initNotesDisplay();
    
    // Handle form submission
    addRoadmapForm.addEventListener('submit', function(e) {
        e.preventDefault();
        
        // Get form values
        const name = document.getElementById('roadmapName').value;
        const link = document.getElementById('roadmapLink').value;
        const notes = document.getElementById('roadmapNotes').value;
        
        // Create new roadmap object
        const newRoadmap = {
            id: Date.now().toString(),
            date: new Date().toLocaleDateString('en-US', {year: 'numeric', month: 'long', day: 'numeric'}),
            name: name,
            link: link,
            notes: notes,
            addedBy: currentUser.username
        };
        
        // Add to local storage
        addRoadmap(newRoadmap);
        
        // Reset form and close modal
        addRoadmapForm.reset();
        addRoadmapModal.style.display = 'none';
        
        // Reload roadmaps
        loadRoadmaps();
    });
}

// Initialize default roadmaps if none exist
function initializeDefaultRoadmaps() {
    const existingRoadmaps = JSON.parse(localStorage.getItem('roadmaps') || '[]');
    
    if (existingRoadmaps.length === 0) {
        const defaultRoadmaps = [
            {
                id: '1',
                date: 'May 20, 2025',
                name: 'Level 0',
                link: 'https://docs.google.com/spreadsheets/d/1nwNw03gRP87ni7-ZH3JJsMGa9bt3URNLGe_osv8zdtM/edit?usp=sharing',
                notes: '',
                addedBy: 'Administrator'
            },
            {
                id: '2',
                date: 'May 20, 2025',
                name: 'Level 1 & 2',
                link: 'https://docs.google.com/spreadsheets/d/1lspiEG_XNOeVcMcAl1cpJ3aBldEKkmedVQ-eEdI28sE/edit?usp=sharing',
                notes: '',
                addedBy: 'Administrator'
            }
        ];
        
        localStorage.setItem('roadmaps', JSON.stringify(defaultRoadmaps));
    }
}

// Add a new roadmap
function addRoadmap(roadmap) {
    const roadmaps = JSON.parse(localStorage.getItem('roadmaps') || '[]');
    roadmaps.push(roadmap);
    localStorage.setItem('roadmaps', JSON.stringify(roadmaps));
}

// Initialize notes display functionality
function initNotesDisplay() {
    const notesDisplayModal = document.getElementById('notesDisplayModal');
    const notesModalTitle = document.getElementById('notesModalTitle');
    const notesContent = document.getElementById('notesContent');
    
    // Global event delegation for notes links
    document.addEventListener('click', function(e) {
        // Check if the clicked element is a notes link or its child icon
        const notesLink = e.target.closest('.notes-link');
        if (notesLink) {
            e.preventDefault();
            
            // Get the roadmap data from the data attributes
            const roadmapId = notesLink.getAttribute('data-roadmap-id');
            const roadmaps = JSON.parse(localStorage.getItem('roadmaps') || '[]');
            const roadmap = roadmaps.find(r => r.id === roadmapId);
            
            if (roadmap && roadmap.notes) {
                // Set the modal content
                notesModalTitle.textContent = `Notes for: ${roadmap.name}`;
                
                // Create formatted content
                notesContent.innerHTML = `
                    <div class="notes-modal-roadmap-name">${roadmap.name}</div>
                    <div>${roadmap.notes}</div>
                    <div class="notes-modal-author">Added by: ${roadmap.addedBy}</div>
                `;
                
                // Show the modal
                notesDisplayModal.style.display = 'block';
            }
        }
    });
}

// Load and display roadmaps
function loadRoadmaps() {
    const roadmapsContainer = document.getElementById('roadmapsContainer');
    const roadmaps = JSON.parse(localStorage.getItem('roadmaps') || '[]');
    
    // Clear the container
    roadmapsContainer.innerHTML = '';
    
    // Add each roadmap to the container
    roadmaps.forEach(roadmap => {
        const roadmapRow = document.createElement('div');
        roadmapRow.className = 'table-row';
        
        roadmapRow.innerHTML = `
            <div class="cell">${roadmap.date}</div>
            <div class="cell">${roadmap.name}</div>
            <div class="cell">
                <a href="${roadmap.link}" target="_blank" class="doc-link">
                    <i class="fas fa-file-pdf"></i>
                </a>
            </div>
            <div class="cell">
                ${roadmap.notes ? `
                <a href="#" class="notes-link" data-roadmap-id="${roadmap.id}">
                    <i class="fas fa-sticky-note"></i>
                </a>` : ''}
            </div>
            <div class="cell">${roadmap.addedBy}</div>
            <div class="cell">
                <button class="delete-btn" data-roadmap-id="${roadmap.id}">
                    <i class="fas fa-trash-alt"></i>
                </button>
            </div>
        `;
        
        roadmapsContainer.appendChild(roadmapRow);
    });
}