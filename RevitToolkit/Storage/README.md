- Tính năng:

	+ Đánh dấu type và các property, field trong type như một schema
	bằng attribute

	+ Tạo schema từ type

	+ Truy xuất schema từ type

	+ Lưu giá trị type vào một data storage và truy xuất giá trị

	+ Lưu giá trị type vào một element và truy xuất giá trị (chưa test)

	+ Xóa schema

- Hạn chế:

	+ Chỉ serialize được field có type chứa parameterless contructor
	
	+ MapFieldAttribute chỉ dùng cho type IDictionary có key type là string,
	value type là simple type Revit cho phép

	+ ArrayFieldAttribute chỉ dùng cho type IList có item type 
	là simple type Revit cho phép

	+ Các field type khác sẽ dùng SerializableFieldAttribute

- Test:

	+ Lưu giá trị type vào một element và truy xuất
	+ Sử dụng giá trị AppId và VendorId + AccessLevel để hạn chế quyền truy cập
	+ Xóa schema => chỉ xóa được entity, không xóa được entity
	+ Trường hợp private field, static field or property

- Phát triển:

	+ Hoàn thiện summary
	+ Kiểm tra dung lượng serializable field, dưới 16mb
	+ Thêm exception
	+ Thiết kế hàm serialize một object bất kì và lưu vào datastorage
