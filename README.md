# **Seminar Building Map**
The primary functionality of this is to provide maps of each building/floor and allow a user to see at a glance if a room is available, a room can be selected to display the daily schedule. A backend administration panel will exist to modify settings, users, etc. Further functionality will be detailed below.

## Map Portion
- There will be an interactive map which displays the current floor plan
    - This map will be colored based on if the room is open, switching soon, or closed
- Clicking a room on the map will open a panel on the right side of the page which hosts a datagrid of events in that room on the current day
- There will be two dropdown lists, one to select the building, and one to select the floor
    - the floor options will be loaded based upon the building selection
    - the page will reload when one of these is changed with the new floorplan map
- There will be a button in the status bar to log in, which will allow admin panel access
- there may also be a button in the status bar for a page with a list of all buildings clicking that page will take you to a datagrid with all rooms in that building clicking a row on that grid will display the full schedule for that room 

## Admin Panel Access
- the admin panel will be accessible via login, but users cannot register themselves
    - admins can register professors for their own building, and superusers can create/promote admins
- there will be three types of accounts:
    - superusers: can assign admins to buildings and create new buildings
    - admins: can assign faculty to their own building and modify classrooms within
    - users: these are professors whom can modify their assigned offices, office hours
- once logged in a main menu will greet users with options based on their permissions
    - normal faculty will only see an option to modify their own office hours
    - admins will have options to: manage classrooms in their building, create users for faculty within their building, reset passwords of users within their building, delete accounts of users within their building
    - superusers will have options to: create new buildings, modify existing buildings, add admins to buildings, create users for buildings, and promote users to admins, change displayed semester on all public facing maps (or delete previous semester availability/events instead we'll discuss it)




