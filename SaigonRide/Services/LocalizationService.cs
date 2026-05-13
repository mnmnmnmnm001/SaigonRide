using System.Collections.Generic;

namespace SaigonRide.Services
{
    public interface ILocalizationService
    {
        string Get(string key, string lang = "en");
        Dictionary<string, string> GetAll(string lang = "en");
        string GetStationName(string englishName, string lang = "en");
    }

    public class LocalizationService : ILocalizationService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _translations = new()
        {
            ["en"] = new()
            {
                ["Home"] = "Home",
                ["Rent"] = "Rent",
                ["Return"] = "Return",
                ["Stations"] = "Stations",
                ["Vehicles"] = "Vehicles",
                ["Users"] = "Users",
                ["Admins"] = "Admins",
                ["FindStation"] = "Find a Station",
                ["SelectStationDesc"] = "Select a station to see available vehicles",
                ["StationName"] = "Station Name",
                ["Bikes"] = "Bikes",
                ["EBikes"] = "E-Bikes",
                ["Scooters"] = "Scooters",
                ["Capacity"] = "Capacity",
                ["Action"] = "Action",
                ["ViewVehicles"] = "View Vehicles",
                ["SlotsFree"] = "slots free",
                ["BackToDashboard"] = "Back to Dashboard",
                ["AvailableVehicles"] = "Available Vehicles",
                ["ChangeStation"] = "Change Station",
                ["RentThisBike"] = "Rent This Bike",
                ["RentThisScooter"] = "Rent This Scooter",
                ["All"] = "All",
                ["Rate"] = "Rate",
                ["VndMin"] = "VND/min",
                ["NoVehicles"] = "No vehicles available here!",
                ["SelectAnother"] = "Select Another Station"
            },
            ["vn"] = new()
            {
                ["Home"] = "Trang Chủ",
                ["Rent"] = "Thuê Xe",
                ["Return"] = "Trả Xe",
                ["Stations"] = "Trạm Xe",
                ["Vehicles"] = "Phương Tiện",
                ["Users"] = "Người Dùng",
                ["Admins"] = "Quản Trị",
                ["FindStation"] = "Tìm Trạm Xe",
                ["SelectStationDesc"] = "Chọn một trạm để xem các xe đang có sẵn",
                ["StationName"] = "Tên Trạm",
                ["Bikes"] = "Xe Đạp",
                ["EBikes"] = "Xe Đạp Điện",
                ["Scooters"] = "Xe Máy Điện",
                ["Capacity"] = "Sức Chứa",
                ["Action"] = "Thao Tác",
                ["ViewVehicles"] = "Xem Xe",
                ["SlotsFree"] = "chỗ trống",
                ["BackToDashboard"] = "Về Bảng Điều Khiển",
                ["AvailableVehicles"] = "Xe Hiện Có",
                ["ChangeStation"] = "Đổi Trạm",
                ["RentThisBike"] = "Thuê Xe Này",
                ["RentThisScooter"] = "Thuê Xe Này",
                ["All"] = "Tất Cả",
                ["Rate"] = "Giá Thuê",
                ["VndMin"] = "VNĐ/phút",
                ["NoVehicles"] = "Không có xe nào ở đây!",
                ["SelectAnother"] = "Chọn Trạm Khác"
            }
        };

        // Station name translations (English -> Vietnamese)
        private readonly Dictionary<string, string> _stationNames = new()
        {
            ["Ben Thanh Market"] = "Chợ Bến Thành",
            ["District 1 Hub"] = "Trung tâm Quận 1",
            ["Saigon Center"] = "Saigon Centre",
            ["Ben Thanh Central Hub"] = "Trạm Trung tâm Bến Thành",
            ["September 23rd Park Station"] = "Trạm Công viên 23/9",
            ["Ham Nghi Transit Center"] = "Trạm Trung chuyển Hàm Nghi",
            ["Saigon Riverside Station"] = "Trạm Bến Bạch Đằng",
            ["Notre Dame Cathedral Stop"] = "Điểm dừng Nhà thờ Đức Bà",
            ["East Gate Terminal"] = "Bến xe Miền Đông",
            ["West Gate Terminal"] = "Bến xe Miền Tây",
            ["New East City Hub"] = "Bến xe Miền Đông Mới",
            ["An Suong Gateway"] = "Cửa ngõ An Sương",
            ["Saigon Railway Station"] = "Ga Sài Gòn",
            ["University Village Hub"] = "Trạm Làng Đại học",
            ["HCMC Tech Campus Station"] = "Trạm Đại học Bách Khoa",
            ["Economy University Stop"] = "Điểm dừng Đại học Kinh tế",
            ["Agriculture Campus Station"] = "Trạm Đại học Nông Lâm",
            ["Thao Diep Expat Village"] = "Khu dân cư Thảo Điền",
            ["Thao Dien Expat Village"] = "Khu dân cư Thảo Điền",
            ["Phu My Hung Center"] = "Trung tâm Phú Mỹ Hưng",
            ["Tan Son Nhat Airport Station"] = "Trạm Sân bay Tân Sơn Nhất",
            ["Chinatown Terminal"] = "Bến xe Chợ Lớn",
            ["Gia Dinh Park Station"] = "Trạm Công viên Gia Định",
            ["Dam Sen Theme Park Stop"] = "Điểm dừng Công viên Đầm Sen"
        };

        public string Get(string key, string lang = "en")
        {
            if (!_translations.ContainsKey(lang)) lang = "en";
            return _translations[lang].GetValueOrDefault(key, key);
        }

        public Dictionary<string, string> GetAll(string lang = "en")
        {
            if (!_translations.ContainsKey(lang)) lang = "en";
            return _translations[lang];
        }

        public string GetStationName(string englishName, string lang = "en")
        {
            if (lang == "vn" && _stationNames.ContainsKey(englishName))
                return _stationNames[englishName];
            return englishName;
        }
    }
}
