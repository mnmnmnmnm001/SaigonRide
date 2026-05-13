`-`-`-`-`-`-`-`-`-`-`-`-`-`-`-`-`-`
## <\> How to run the project
1.  Open visual studio
2.  Clone the repository
3.  Run ```Program.cs```
4.  Wait ~33 second (yes, it's long) for initalization

## |/|\| Admin login infromation
Username: ```qwe```
Password: ```1234560```
There is only 1 admin account currently

## </> How to ###use### the project
1.  Normal user will choose between rent or return. Admin will login using login as admin button

# Rent
2.1  If user choose rent, user will then choose a vehicle in a new screen
3.1  User will then enter all information (all are necessary) and submit
4.1.1  If Invalid information, the screen will show where is the invalid informaton
4.1.2  If correct information, bank account will sent confirm sent money to user via their bank app in their phone, the system will wait 70 seconds for confirmation (not implemented)
5.1.1 If user not confirm within 70 seconds, the system will automatically return to the main screen (not implemented)
5.1.2 If user confirm within 70 seconds (not implemented), the system will automatically move to receipt screen and automatically return to the main screen after 7 seconds. The system also sent encryted bank account number and other necessary information to the main database

# Return
2.2  If user choose return, user will then choose a station in a new screen
3.2  User will then enter all information (all are necessary) and submit
4.2.1  If Invalid information, the screen will show where is the invalid informaton
4.2.2  If correct information, bank account of user that have been kept with vehicle code will be automatically take more/refund depending on how many minutes since showed receipt screen and discount depending on current station capacity/max capacity < 20%. Finally, the system will sutomatically return to the main screen after 7 seconds (not implemented)

# Admin
2.3  If user choose login as admin, the system will show login screen
3.3  User will then enter all information (all are necessary) and submit
4.3  If correct information, the user can now access all admin functionality, user can see revenue report or station report. They can CRUD: transaction (user), station, vehicle.
