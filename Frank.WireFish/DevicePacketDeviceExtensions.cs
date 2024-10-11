using System.Security;
using System.Net;
using System.Net.NetworkInformation;

using Bitvantage.Ethernet;

using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace Frank.WireFish;


    /// <summary>
    /// Provides extension methods for the DevicePacket record to access device information.
    /// </summary>
    public static class DevicePacketDeviceExtensions
    {
        /// <summary>
        /// Gets the name of the capture device.
        /// </summary>
        public static string GetDeviceName(this DevicePacket devicePacket)
        {
            return devicePacket.Device.Name;
        }

        /// <summary>
        /// Gets the description of the capture device.
        /// </summary>
        public static string GetDeviceDescription(this DevicePacket devicePacket)
        {
            return devicePacket.Device.Description;
        }

        /// <summary>
        /// Gets the MAC address of the capture device.
        /// </summary>
        public static PhysicalAddress GetDeviceMacAddress(this DevicePacket devicePacket)
        {
            if (devicePacket.Device is LibPcapLiveDevice liveDevice)
            {
                return liveDevice.MacAddress;
            }
            return null;
        }

        /// <summary>
        /// Gets the list of IP addresses assigned to the capture device.
        /// </summary>
        public static IEnumerable<IPAddress> GetDeviceIpAddresses(this DevicePacket devicePacket)
        {
            if (devicePacket.Device is LibPcapLiveDevice liveDevice)
            {
                return liveDevice.Addresses
                    .Where(addr => addr.Addr != null && addr.Addr.ipAddress != null)
                    .Select(addr => addr.Addr.ipAddress);
            }
            return Enumerable.Empty<IPAddress>();
        }

        /// <summary>
        /// Gets the link layer type of the capture device.
        /// </summary>
        public static LinkLayers GetDeviceLinkLayerType(this DevicePacket devicePacket)
        {
            return devicePacket.Device.LinkType;
        }

        /// <summary>
        /// Gets the statistics of the capture device.
        /// </summary>
        public static ICaptureStatistics GetDeviceStatistics(this DevicePacket devicePacket)
        {
            return devicePacket.Device.Statistics;
        }

        /// <summary>
        /// Gets the capture filter applied to the device.
        /// </summary>
        public static string GetDeviceCaptureFilter(this DevicePacket devicePacket)
        {
            return devicePacket.Device.Filter;
        }

        /// <summary>
        /// Gets the vendor name of the capture device based on the MAC address.
        /// </summary>
        public static string GetDeviceVendor(this DevicePacket devicePacket)
        {
            var macAddress = devicePacket.GetDeviceMacAddress();
            return OuiDatabase.Instance[MacAddress.Parse(macAddress.ToString())].Organization;
        }

        /// <summary>
        /// Checks if the capture device is a wireless interface.
        /// </summary>
        public static bool IsWirelessDevice(this DevicePacket devicePacket)
        {
            if (devicePacket.Device is LibPcapLiveDevice liveDevice)
            {
                return liveDevice.Interface.FriendlyName.Contains("Wireless", StringComparison.OrdinalIgnoreCase) ||
                       liveDevice.Interface.FriendlyName.Contains("Wi-Fi", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        /// <summary>
        /// Gets the friendly name of the network interface.
        /// </summary>
        public static string GetDeviceFriendlyName(this DevicePacket devicePacket)
        {
            if (devicePacket.Device is LibPcapLiveDevice liveDevice)
            {
                return liveDevice.Interface.FriendlyName;
            }
            return null;
        }
        
        
        /// <summary>
        /// Gets the network interface identifier of the capture device.
        /// </summary>
        public static string GetDeviceId(this DevicePacket devicePacket)
        {
            if (devicePacket.Device is LibPcapLiveDevice liveDevice)
            {
                return liveDevice.Interface.Name;
            }
            return null;
        }

        /// <summary>
        /// Gets the network interface associated with the capture device.
        /// </summary>
        public static NetworkInterface GetNetworkInterface(this DevicePacket devicePacket)
        {
            if (devicePacket.Device is LibPcapLiveDevice liveDevice)
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface networkInterface in interfaces)
                {
                    if (networkInterface.Id == liveDevice.Interface.Name)
                    {
                        return networkInterface;
                    }
                }
            }
            
            throw new SecurityException("Network interface not found. T");
        }

        /// <summary>
        /// Gets the MTU (Maximum Transmission Unit) size of the network interface.
        /// </summary>
        public static int? GetDeviceMtu(this DevicePacket devicePacket)
        {
            return GetNetworkInterface(devicePacket).GetIPProperties().GetIPv4Properties().Mtu;
        }

        /// <summary>
        /// Checks if the device is a loopback interface.
        /// </summary>
        public static bool IsLoopbackDevice(this DevicePacket devicePacket)
        {
            return GetNetworkInterface(devicePacket).NetworkInterfaceType == NetworkInterfaceType.Loopback;
        }
    }

