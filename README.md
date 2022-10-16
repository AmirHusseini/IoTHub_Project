# IoTHub_Project

## Description
An iot hub project using azure function, web api, wpf and console. Remotely control your devices(Lamps, Fans, Tv etc) from anywhere. You can add or remove devices. Azure function and web api are currently published on Microsoft Azure.
### WebApi
Using the rest api you can get all your devices, one device, delete or add. This web api is currently running on microsoft azure but you can add your own in the project. 
### Azure function
i have used azure function to learn more and make some part of the code easier, there are two functions, one http trigger to get a device connectionstring and one grid trigger to get eventdata.

### WPFApp
UI framework as desktop client applications to control devices. 

<img src="./githubimages/HomeViewModel.jpg" width=450 height=250><img src="./githubimages/KitchenViewModel.jpg" width=450 height=250><img src="./githubimages/LivingroomViewModel.jpg" width=450 height=250><img src="./githubimages/DeviceManagement.jpg" width=450 height=250>

### MultiDevices Console
It's a simple console app acting as device where you can write the id of any device and get true or false deviceState from invokemthod in WPFApp. It also sends temperature and humidity telemtry to client and you can get using eventdata.

<img src="./githubimages/DeviceConsole.jpg" width=450 height=250>


### Device.Fan.Livingroom
It's a wpf app acting as a device fan in the livingroom using inovkmethod you can turn on or off the fan. 

<img src="./githubimages/DeviceFan_livingroom.jpg" width=200 height=250>
