# Escc.PhotoConsent

This project is an admin application and E-Form to allow participants to give consent for an ESCC photo shoot or video.


## Development setup steps

1. From an Administrator command prompt, run `app-setup-dev.cmd` to set up a site in IIS.
2. Build the solution
3. Grant modify permissions to the application pool account on the web root folder and all children
4. In `~\web.config` set the `ServerUrl` app setting to the host url.
5. In `~\web.config` set the `EmailFrom` app setting to the address you wish success emails to be from.
6. In `~\web.config` set the `Authorize` app setting to contain the AD role Groups you wish to have access to the application
7. In `~\web.config` add the connection strings for `PhotoConsentDB` and `Escc.Services.Azure.EmailQueue`
