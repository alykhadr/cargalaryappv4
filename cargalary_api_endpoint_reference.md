# CarGalary.Api Endpoint Reference
- **OpenAPI:** 3.0.4
- **Version:** 1.0
- **Total endpoints:** 27
## Endpoints
### POST `/api/auth/register`
- **Tag:** Auth
- **Sample URL:** `/api/auth/register`
- **Parameters:**
  - Email (query, string, optional)
  - UserName (query, string, optional)
  - Password (query, string, optional)
  - NameEn (query, string, optional)
  - NameAr (query, string, optional)
  - BranchId (query, integer(int32), optional)
  - Roles (query, array[string], optional)
  - EmployeeNo (query, string, optional)
  - NationalId (query, string, optional)
  - JobTitle (query, string, optional)
  - DepartmentId (query, integer(int32), optional)
  - HireDate (query, string(date-time), optional)
  - TerminationDate (query, string(date-time), optional)
  - EmploymentStatus (query, string, optional)
  - WorkEmail (query, string, optional)
  - WorkPhone (query, string, optional)
  - Extension (query, string, optional)
  - DateOfBirth (query, string(date-time), optional)
  - Gender (query, string, optional)
  - Nationality (query, string, optional)
  - AddressLine1 (query, string, optional)
  - AddressLine2 (query, string, optional)
  - City (query, string, optional)
  - Region (query, string, optional)
  - PostalCode (query, string, optional)
- **Request Content-Type:** multipart/form-data
- **Request Schema:** `object{ProfileImage}`
**Request Example**

```json
{
  "ProfileImage": "<binary-file>"
}
```

- **Response Status:** 200
- **Response Content-Type:** None specified
- **Response Schema:** `OK`
**Response Example**

```json
{
  "description": "OK"
}
```

### POST `/api/auth/login`
- **Tag:** Auth
- **Sample URL:** `/api/auth/login`
- **Parameters:**
  - None
- **Request Content-Type:** application/json
- **Request Schema:** `LoginRequest`
**Request Example**

```json
{
  "userName": "string",
  "password": "string",
  "rememberMe": true
}
```

- **Response Status:** 200
- **Response Content-Type:** None specified
- **Response Schema:** `OK`
**Response Example**

```json
{
  "description": "OK"
}
```

### POST `/api/auth/forgot-password`
- **Tag:** Auth
- **Sample URL:** `/api/auth/forgot-password`
- **Parameters:**
  - None
- **Request Content-Type:** application/json
- **Request Schema:** `ForgotPasswordRequest`
**Request Example**

```json
{
  "userNameOrEmail": "string"
}
```

- **Response Status:** 200
- **Response Content-Type:** None specified
- **Response Schema:** `OK`
**Response Example**

```json
{
  "description": "OK"
}
```

### POST `/api/auth/reset-password`
- **Tag:** Auth
- **Sample URL:** `/api/auth/reset-password`
- **Parameters:**
  - None
- **Request Content-Type:** application/json
- **Request Schema:** `ResetPasswordRequest`
**Request Example**

```json
{
  "userNameOrEmail": "string",
  "token": "string",
  "newPassword": "string"
}
```

- **Response Status:** 200
- **Response Content-Type:** None specified
- **Response Schema:** `OK`
**Response Example**

```json
{
  "description": "OK"
}
```

### GET `/api/branches`
- **Tag:** Branches
- **Sample URL:** `/api/branches`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[BranchResponseDto]`
**Response Example**

```json
[
  {
    "descriptionAr": "string",
    "descriptionEn": "string",
    "mobileNo": "string",
    "email": "string",
    "branchNameAr": "string",
    "branchNameEn": "string",
    "createdBy": "string",
    "address": "string",
    "whatsUpNo": "string",
    "latitute": "string",
    "longtute": "string",
    "isAvailable": true,
    "id": 1,
    "branchWorkingDaysResponseDtos": [
      {
        "id": 1,
        "isAvailable": true,
        "dayAr": "string",
        "dayEn": "string",
        "workingFrom": 1,
        "workingTo": 1,
        "timeType": "string"
      }
    ]
  }
]
```

### GET `/api/branches/{id}`
- **Tag:** Branches
- **Sample URL:** `/api/branches/1`
- **Parameters:**
  - id (path, integer(int32), required)
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `BranchResponseDto`
**Response Example**

```json
{
  "descriptionAr": "string",
  "descriptionEn": "string",
  "mobileNo": "string",
  "email": "string",
  "branchNameAr": "string",
  "branchNameEn": "string",
  "createdBy": "string",
  "address": "string",
  "whatsUpNo": "string",
  "latitute": "string",
  "longtute": "string",
  "isAvailable": true,
  "id": 1,
  "branchWorkingDaysResponseDtos": [
    {
      "id": 1,
      "isAvailable": true,
      "dayAr": "string",
      "dayEn": "string",
      "workingFrom": 1,
      "workingTo": 1,
      "timeType": "string"
    }
  ]
}
```

### GET `/api/Brand`
- **Tag:** Brand
- **Sample URL:** `/api/Brand`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[BrandDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "nameAr": "string",
    "nameEn": "string",
    "imageUrl": "string",
    "createdBy": "string"
  }
]
```

### GET `/api/Brand/{brandId}/models`
- **Tag:** Brand
- **Sample URL:** `/api/Brand/1/models`
- **Parameters:**
  - brandId (path, integer(int32), required)
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[CarModelByBrandResponseDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "nameAr": "string",
    "nameEn": "string",
    "brandId": 1,
    "isAvailable": true,
    "createdAt": "2026-05-11T00:00:00Z"
  }
]
```

### GET `/api/version`
- **Tag:** CarGalary.Api
- **Sample URL:** `/api/version`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** None specified
- **Response Schema:** `OK`
**Response Example**

```json
{
  "description": "OK"
}
```

### GET `/api/Cars`
- **Tag:** Cars
- **Sample URL:** `/api/Cars`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[CarApiResponseDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "nameAr": "string",
    "nameEn": "string",
    "modelId": 1,
    "modelNameAr": "string",
    "modelNameEn": "string",
    "brandId": 1,
    "brandNameAr": "string",
    "brandNameEn": "string",
    "typeId": 1,
    "typeNameAr": "string",
    "typeNameEn": "string",
    "branchId": 1,
    "branchNameAr": "string",
    "branchNameEn": "string",
    "year": 1,
    "mileage": 1,
    "vat": 0.0,
    "conditionId": 1,
    "conditionNameAr": "string",
    "conditionNameEn": "string",
    "seatingCapacity": 1,
    "weelSizeInch": "string",
    "fuelTankCapacityLiter": 0.0,
    "trimLevel": 1,
    "trimLevelNameAr": "string",
    "trimLevelNameEn": "string",
    "vehicleClass": 1,
    "vehicleClassNameAr": "string",
    "vehicleClassNameEn": "string",
    "plateNumberAr": "string",
    "plateNumberEn": "string",
    "transmisionType": 1,
    "transmisionTypeNameAr": "string",
    "transmisionTypeNameEn": "string",
    "drivetrain": 1,
    "drivetrainNameAr": "string",
    "drivetrainNameEn": "string",
    "cylenders": 1,
    "fuelType": 1,
    "fuelTypeNameAr": "string",
    "fuelTypeNameEn": "string",
    "manufactureCountryId": 1,
    "manufactureCountryNameAr": "string",
    "manufactureCountryNameEn": "string",
    "enginNumber": "string",
    "descriptionAr": "string",
    "descriptionEn": "string",
    "createdAt": "2026-05-11T00:00:00Z",
    "createdBy": "string",
    "isAvailable": true,
    "features": [
      {
        "id": 1,
        "nameAr": "string",
        "nameEn": "string",
        "isAvailable": true
      }
    ],
    "colors": [
      {
        "carId": 1,
        "colorId": 1,
        "colorNameAr": "string",
        "colorNameEn": "string",
        "colorCode": "string",
        "colorStatus": 1,
        "colorStatusDetailCode": "string",
        "colorStatusNameAr": "string",
        "colorStatusNameEn": "string",
        "stockQuantity": 1,
        "colorImageUrl": "string",
        "pricingPerColor": 0.0,
        "pricePefore": 0.0,
        "vatAmount": 0.0,
        "discount": 0.0,
        "discountType": 1,
        "totalPrice": 0.0,
        "isAvailable": true
      }
    ],
    "extraDetails": [
      {
        "id": 1,
        "nameAr": "string",
        "nameEn": "string",
        "descriptionEn": "string",
        "descriptionAr": "string",
        "carExtraDetailsType": 1,
        "carExtraDetailsTypeNameAr": "string",
        "carExtraDetailsTypeNameEn": "string",
        "createdBy": "string",
        "isAvailable": true,
        "carId": 1
      }
    ],
    "galleryImages": [
      {
        "id": 1,
        "carId": 1,
        "imageUrl": "string",
        "imageType": 1,
        "imageTypeNameAr": "string",
        "imageTypeNameEn": "string",
        "isPrimary": true,
        "createdBy": "string",
        "isAvailable": true
      }
    ]
  }
]
```

### GET `/api/Cars/latest`
- **Tag:** Cars
- **Sample URL:** `/api/Cars/latest`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[CarApiResponseDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "nameAr": "string",
    "nameEn": "string",
    "modelId": 1,
    "modelNameAr": "string",
    "modelNameEn": "string",
    "brandId": 1,
    "brandNameAr": "string",
    "brandNameEn": "string",
    "typeId": 1,
    "typeNameAr": "string",
    "typeNameEn": "string",
    "branchId": 1,
    "branchNameAr": "string",
    "branchNameEn": "string",
    "year": 1,
    "mileage": 1,
    "vat": 0.0,
    "conditionId": 1,
    "conditionNameAr": "string",
    "conditionNameEn": "string",
    "seatingCapacity": 1,
    "weelSizeInch": "string",
    "fuelTankCapacityLiter": 0.0,
    "trimLevel": 1,
    "trimLevelNameAr": "string",
    "trimLevelNameEn": "string",
    "vehicleClass": 1,
    "vehicleClassNameAr": "string",
    "vehicleClassNameEn": "string",
    "plateNumberAr": "string",
    "plateNumberEn": "string",
    "transmisionType": 1,
    "transmisionTypeNameAr": "string",
    "transmisionTypeNameEn": "string",
    "drivetrain": 1,
    "drivetrainNameAr": "string",
    "drivetrainNameEn": "string",
    "cylenders": 1,
    "fuelType": 1,
    "fuelTypeNameAr": "string",
    "fuelTypeNameEn": "string",
    "manufactureCountryId": 1,
    "manufactureCountryNameAr": "string",
    "manufactureCountryNameEn": "string",
    "enginNumber": "string",
    "descriptionAr": "string",
    "descriptionEn": "string",
    "createdAt": "2026-05-11T00:00:00Z",
    "createdBy": "string",
    "isAvailable": true,
    "features": [
      {
        "id": 1,
        "nameAr": "string",
        "nameEn": "string",
        "isAvailable": true
      }
    ],
    "colors": [
      {
        "carId": 1,
        "colorId": 1,
        "colorNameAr": "string",
        "colorNameEn": "string",
        "colorCode": "string",
        "colorStatus": 1,
        "colorStatusDetailCode": "string",
        "colorStatusNameAr": "string",
        "colorStatusNameEn": "string",
        "stockQuantity": 1,
        "colorImageUrl": "string",
        "pricingPerColor": 0.0,
        "pricePefore": 0.0,
        "vatAmount": 0.0,
        "discount": 0.0,
        "discountType": 1,
        "totalPrice": 0.0,
        "isAvailable": true
      }
    ],
    "extraDetails": [
      {
        "id": 1,
        "nameAr": "string",
        "nameEn": "string",
        "descriptionEn": "string",
        "descriptionAr": "string",
        "carExtraDetailsType": 1,
        "carExtraDetailsTypeNameAr": "string",
        "carExtraDetailsTypeNameEn": "string",
        "createdBy": "string",
        "isAvailable": true,
        "carId": 1
      }
    ],
    "galleryImages": [
      {
        "id": 1,
        "carId": 1,
        "imageUrl": "string",
        "imageType": 1,
        "imageTypeNameAr": "string",
        "imageTypeNameEn": "string",
        "isPrimary": true,
        "createdBy": "string",
        "isAvailable": true
      }
    ]
  }
]
```

### GET `/api/Cars/{id}`
- **Tag:** Cars
- **Sample URL:** `/api/Cars/1`
- **Parameters:**
  - id (path, integer(int32), required)
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `CarApiResponseDto`
**Response Example**

```json
{
  "id": 1,
  "nameAr": "string",
  "nameEn": "string",
  "modelId": 1,
  "modelNameAr": "string",
  "modelNameEn": "string",
  "brandId": 1,
  "brandNameAr": "string",
  "brandNameEn": "string",
  "typeId": 1,
  "typeNameAr": "string",
  "typeNameEn": "string",
  "branchId": 1,
  "branchNameAr": "string",
  "branchNameEn": "string",
  "year": 1,
  "mileage": 1,
  "vat": 0.0,
  "conditionId": 1,
  "conditionNameAr": "string",
  "conditionNameEn": "string",
  "seatingCapacity": 1,
  "weelSizeInch": "string",
  "fuelTankCapacityLiter": 0.0,
  "trimLevel": 1,
  "trimLevelNameAr": "string",
  "trimLevelNameEn": "string",
  "vehicleClass": 1,
  "vehicleClassNameAr": "string",
  "vehicleClassNameEn": "string",
  "plateNumberAr": "string",
  "plateNumberEn": "string",
  "transmisionType": 1,
  "transmisionTypeNameAr": "string",
  "transmisionTypeNameEn": "string",
  "drivetrain": 1,
  "drivetrainNameAr": "string",
  "drivetrainNameEn": "string",
  "cylenders": 1,
  "fuelType": 1,
  "fuelTypeNameAr": "string",
  "fuelTypeNameEn": "string",
  "manufactureCountryId": 1,
  "manufactureCountryNameAr": "string",
  "manufactureCountryNameEn": "string",
  "enginNumber": "string",
  "descriptionAr": "string",
  "descriptionEn": "string",
  "createdAt": "2026-05-11T00:00:00Z",
  "createdBy": "string",
  "isAvailable": true,
  "features": [
    {
      "id": 1,
      "nameAr": "string",
      "nameEn": "string",
      "isAvailable": true
    }
  ],
  "colors": [
    {
      "carId": 1,
      "colorId": 1,
      "colorNameAr": "string",
      "colorNameEn": "string",
      "colorCode": "string",
      "colorStatus": 1,
      "colorStatusDetailCode": "string",
      "colorStatusNameAr": "string",
      "colorStatusNameEn": "string",
      "stockQuantity": 1,
      "colorImageUrl": "string",
      "pricingPerColor": 0.0,
      "pricePefore": 0.0,
      "vatAmount": 0.0,
      "discount": 0.0,
      "discountType": 1,
      "totalPrice": 0.0,
      "isAvailable": true
    }
  ],
  "extraDetails": [
    {
      "id": 1,
      "nameAr": "string",
      "nameEn": "string",
      "descriptionEn": "string",
      "descriptionAr": "string",
      "carExtraDetailsType": 1,
      "carExtraDetailsTypeNameAr": "string",
      "carExtraDetailsTypeNameEn": "string",
      "createdBy": "string",
      "isAvailable": true,
      "carId": 1
    }
  ],
  "galleryImages": [
    {
      "id": 1,
      "carId": 1,
      "imageUrl": "string",
      "imageType": 1,
      "imageTypeNameAr": "string",
      "imageTypeNameEn": "string",
      "isPrimary": true,
      "createdBy": "string",
      "isAvailable": true
    }
  ]
}
```

### GET `/api/Cars/by-model/{modelId}`
- **Tag:** Cars
- **Sample URL:** `/api/Cars/by-model/1`
- **Parameters:**
  - modelId (path, integer(int32), required)
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[CarApiResponseDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "nameAr": "string",
    "nameEn": "string",
    "modelId": 1,
    "modelNameAr": "string",
    "modelNameEn": "string",
    "brandId": 1,
    "brandNameAr": "string",
    "brandNameEn": "string",
    "typeId": 1,
    "typeNameAr": "string",
    "typeNameEn": "string",
    "branchId": 1,
    "branchNameAr": "string",
    "branchNameEn": "string",
    "year": 1,
    "mileage": 1,
    "vat": 0.0,
    "conditionId": 1,
    "conditionNameAr": "string",
    "conditionNameEn": "string",
    "seatingCapacity": 1,
    "weelSizeInch": "string",
    "fuelTankCapacityLiter": 0.0,
    "trimLevel": 1,
    "trimLevelNameAr": "string",
    "trimLevelNameEn": "string",
    "vehicleClass": 1,
    "vehicleClassNameAr": "string",
    "vehicleClassNameEn": "string",
    "plateNumberAr": "string",
    "plateNumberEn": "string",
    "transmisionType": 1,
    "transmisionTypeNameAr": "string",
    "transmisionTypeNameEn": "string",
    "drivetrain": 1,
    "drivetrainNameAr": "string",
    "drivetrainNameEn": "string",
    "cylenders": 1,
    "fuelType": 1,
    "fuelTypeNameAr": "string",
    "fuelTypeNameEn": "string",
    "manufactureCountryId": 1,
    "manufactureCountryNameAr": "string",
    "manufactureCountryNameEn": "string",
    "enginNumber": "string",
    "descriptionAr": "string",
    "descriptionEn": "string",
    "createdAt": "2026-05-11T00:00:00Z",
    "createdBy": "string",
    "isAvailable": true,
    "features": [
      {
        "id": 1,
        "nameAr": "string",
        "nameEn": "string",
        "isAvailable": true
      }
    ],
    "colors": [
      {
        "carId": 1,
        "colorId": 1,
        "colorNameAr": "string",
        "colorNameEn": "string",
        "colorCode": "string",
        "colorStatus": 1,
        "colorStatusDetailCode": "string",
        "colorStatusNameAr": "string",
        "colorStatusNameEn": "string",
        "stockQuantity": 1,
        "colorImageUrl": "string",
        "pricingPerColor": 0.0,
        "pricePefore": 0.0,
        "vatAmount": 0.0,
        "discount": 0.0,
        "discountType": 1,
        "totalPrice": 0.0,
        "isAvailable": true
      }
    ],
    "extraDetails": [
      {
        "id": 1,
        "nameAr": "string",
        "nameEn": "string",
        "descriptionEn": "string",
        "descriptionAr": "string",
        "carExtraDetailsType": 1,
        "carExtraDetailsTypeNameAr": "string",
        "carExtraDetailsTypeNameEn": "string",
        "createdBy": "string",
        "isAvailable": true,
        "carId": 1
      }
    ],
    "galleryImages": [
      {
        "id": 1,
        "carId": 1,
        "imageUrl": "string",
        "imageType": 1,
        "imageTypeNameAr": "string",
        "imageTypeNameEn": "string",
        "isPrimary": true,
        "createdBy": "string",
        "isAvailable": true
      }
    ]
  }
]
```

### GET `/api/company-information`
- **Tag:** CompanyInformation
- **Sample URL:** `/api/company-information`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** None specified
- **Response Schema:** `OK`
**Response Example**

```json
{
  "description": "OK"
}
```

### GET `/api/contact-sales`
- **Tag:** ContactSalesOfficer
- **Sample URL:** `/api/contact-sales`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[ContactSalesOfficerResponseDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "contactValue": "string",
    "contactType": 1,
    "contactTypeNameAr": "string",
    "contactTypeNameEn": "string",
    "contactIconUrl": "string",
    "createdBy": "string",
    "isAvailable": true,
    "createdAt": "2026-05-11T00:00:00Z",
    "branchId": 1
  }
]
```

### GET `/api/contact-us`
- **Tag:** ContactUs
- **Sample URL:** `/api/contact-us`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[ContactUsResponseDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "contactValue": "string",
    "contactType": 1,
    "contactTypeNameAr": "string",
    "contactTypeNameEn": "string",
    "contactIconUrl": "string",
    "messageAr": "string",
    "messageEn": "string",
    "createdBy": "string",
    "isAvailable": true,
    "createdAt": "2026-05-11T00:00:00Z"
  }
]
```

### GET `/api/faqs`
- **Tag:** FAQs
- **Sample URL:** `/api/faqs`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[FAQResponseDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "titleAr": "string",
    "titleEn": "string",
    "descriptionAr": "string",
    "descriptionEn": "string",
    "order": 1,
    "isAvailable": true
  }
]
```

### GET `/api/favorites`
- **Tag:** Favorites
- **Sample URL:** `/api/favorites`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[UserFavoriteAdminResponseDto]`
**Response Example**

```json
[
  {
    "userId": "00000000-0000-0000-0000-000000000000",
    "carId": 1,
    "carNameAr": "string",
    "carNameEn": "string",
    "modelId": 1,
    "modelNameAr": "string",
    "modelNameEn": "string",
    "brandId": 1,
    "brandNameAr": "string",
    "brandNameEn": "string",
    "notes": "string",
    "priority": 1,
    "createdAt": "2026-05-11T00:00:00Z"
  }
]
```

### POST `/api/favorites`
- **Tag:** Favorites
- **Sample URL:** `/api/favorites`
- **Parameters:**
  - None
- **Request Content-Type:** application/json
- **Request Schema:** `CreateMyFavoriteRequest`
**Request Example**

```json
{
  "carId": 1,
  "notes": "string",
  "priority": 1
}
```

- **Response Status:** 200
- **Response Content-Type:** None specified
- **Response Schema:** `OK`
**Response Example**

```json
{
  "description": "OK"
}
```

### GET `/api/favorites/my`
- **Tag:** Favorites
- **Sample URL:** `/api/favorites/my`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[UserFavoriteAdminResponseDto]`
**Response Example**

```json
[
  {
    "userId": "00000000-0000-0000-0000-000000000000",
    "carId": 1,
    "carNameAr": "string",
    "carNameEn": "string",
    "modelId": 1,
    "modelNameAr": "string",
    "modelNameEn": "string",
    "brandId": 1,
    "brandNameAr": "string",
    "brandNameEn": "string",
    "notes": "string",
    "priority": 1,
    "createdAt": "2026-05-11T00:00:00Z"
  }
]
```

### GET `/api/offer`
- **Tag:** Offer
- **Sample URL:** `/api/offer`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[OfferResponseDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "offerImageUrl": "string",
    "offerNameAr": "string",
    "offerNameEn": "string",
    "descriptionAr": "string",
    "descriptionEn": "string",
    "expiredAt": "2026-05-11T00:00:00Z",
    "isAvailable": true
  }
]
```

### GET `/api/packages`
- **Tag:** Packages
- **Sample URL:** `/api/packages`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[PackageResponseDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "nameAr": "string",
    "nameEn": "string",
    "imageUrl": "string",
    "createdBy": "string",
    "isAvailable": true
  }
]
```

### POST `/api/profile/update-email`
- **Tag:** Profile
- **Sample URL:** `/api/profile/update-email`
- **Parameters:**
  - None
- **Request Content-Type:** application/json
- **Request Schema:** `string`
**Request Example**

```json
"string"
```

- **Response Status:** 200
- **Response Content-Type:** None specified
- **Response Schema:** `OK`
**Response Example**

```json
{
  "description": "OK"
}
```

### POST `/api/profile/update-username`
- **Tag:** Profile
- **Sample URL:** `/api/profile/update-username`
- **Parameters:**
  - None
- **Request Content-Type:** application/json
- **Request Schema:** `string`
**Request Example**

```json
"string"
```

- **Response Status:** 200
- **Response Content-Type:** None specified
- **Response Schema:** `OK`
**Response Example**

```json
{
  "description": "OK"
}
```

### POST `/api/Requests`
- **Tag:** Requests
- **Sample URL:** `/api/Requests`
- **Parameters:**
  - None
- **Request Content-Type:** application/json
- **Request Schema:** `CreateRequestDto`
**Request Example**

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "vehicleOwnerType": 1,
  "name": "string",
  "email": "string",
  "mobileNo": "string",
  "carId": 1,
  "colorId": 1,
  "paymentMethod": 1,
  "regionId": 1,
  "cityId": 1,
  "notes": "string"
}
```

- **Response Status:** 200
- **Response Content-Type:** None specified
- **Response Schema:** `OK`
**Response Example**

```json
{
  "description": "OK"
}
```

### GET `/api/services`
- **Tag:** Services
- **Sample URL:** `/api/services`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[ServicesResponseDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "nameAr": "string",
    "nameEn": "string",
    "descriptionAr": "string",
    "descriptionEn": "string",
    "discount": 0.0,
    "isPercentage": true,
    "serviceImageUrl": "string",
    "isAvailable": true
  }
]
```

### GET `/api/subscribers`
- **Tag:** Subscribers
- **Sample URL:** `/api/subscribers`
- **Parameters:**
  - None
- **Request Content-Type:** None
- **Request Schema:** `None`
**Request Example**

```
None
```

- **Response Status:** 200
- **Response Content-Type:** application/json
- **Response Schema:** `array[MemberServiceResponseDto]`
**Response Example**

```json
[
  {
    "id": 1,
    "nameAr": "string",
    "nameEn": "string",
    "descriptionAr": "string",
    "descriptionEn": "string",
    "imageUrl": "string",
    "isAvailable": true
  }
]
```

## Schemas
### `BranchResponseDto`
**Fields**

- descriptionAr: string | nullable
- descriptionEn: string | nullable
- mobileNo: string | nullable
- email: string | nullable
- branchNameAr: string | nullable
- branchNameEn: string | nullable
- createdBy: string | nullable
- address: string | nullable
- whatsUpNo: string | nullable
- latitute: string | nullable
- longtute: string | nullable
- isAvailable: boolean
- id: integer(int32)
- branchWorkingDaysResponseDtos: array[BranchWorkingDaysResponseDto] | nullable
-   branchWorkingDaysResponseDtos[].id: integer(int32)
-   branchWorkingDaysResponseDtos[].isAvailable: boolean
-   branchWorkingDaysResponseDtos[].dayAr: string | nullable
-   branchWorkingDaysResponseDtos[].dayEn: string | nullable
-   branchWorkingDaysResponseDtos[].workingFrom: integer(int32) | nullable
-   branchWorkingDaysResponseDtos[].workingTo: integer(int32) | nullable
-   branchWorkingDaysResponseDtos[].timeType: string | nullable

**Example**

```json
{
  "descriptionAr": "string",
  "descriptionEn": "string",
  "mobileNo": "string",
  "email": "string",
  "branchNameAr": "string",
  "branchNameEn": "string",
  "createdBy": "string",
  "address": "string",
  "whatsUpNo": "string",
  "latitute": "string",
  "longtute": "string",
  "isAvailable": true,
  "id": 1,
  "branchWorkingDaysResponseDtos": [
    {
      "id": 1,
      "isAvailable": true,
      "dayAr": "string",
      "dayEn": "string",
      "workingFrom": 1,
      "workingTo": 1,
      "timeType": "string"
    }
  ]
}
```

### `BranchWorkingDaysResponseDto`
**Fields**

- id: integer(int32)
- isAvailable: boolean
- dayAr: string | nullable
- dayEn: string | nullable
- workingFrom: integer(int32) | nullable
- workingTo: integer(int32) | nullable
- timeType: string | nullable

**Example**

```json
{
  "id": 1,
  "isAvailable": true,
  "dayAr": "string",
  "dayEn": "string",
  "workingFrom": 1,
  "workingTo": 1,
  "timeType": "string"
}
```

### `BrandDto`
**Fields**

- id: integer(int32)
- nameAr: string | nullable
- nameEn: string | nullable
- imageUrl: string | nullable
- createdBy: string | nullable

**Example**

```json
{
  "id": 1,
  "nameAr": "string",
  "nameEn": "string",
  "imageUrl": "string",
  "createdBy": "string"
}
```

### `CarApiResponseDto`
**Fields**

- id: integer(int32)
- nameAr: string | nullable
- nameEn: string | nullable
- modelId: integer(int32)
- modelNameAr: string | nullable
- modelNameEn: string | nullable
- brandId: integer(int32) | nullable
- brandNameAr: string | nullable
- brandNameEn: string | nullable
- typeId: integer(int32)
- typeNameAr: string | nullable
- typeNameEn: string | nullable
- branchId: integer(int32)
- branchNameAr: string | nullable
- branchNameEn: string | nullable
- year: integer(int32)
- mileage: integer(int32)
- vat: number(double) | nullable
- conditionId: integer(int32) | nullable
- conditionNameAr: string | nullable
- conditionNameEn: string | nullable
- seatingCapacity: integer(int32) | nullable
- weelSizeInch: string | nullable
- fuelTankCapacityLiter: number(double) | nullable
- trimLevel: integer(int32) | nullable
- trimLevelNameAr: string | nullable
- trimLevelNameEn: string | nullable
- vehicleClass: integer(int32) | nullable
- vehicleClassNameAr: string | nullable
- vehicleClassNameEn: string | nullable
- plateNumberAr: string | nullable
- plateNumberEn: string | nullable
- transmisionType: integer(int32) | nullable
- transmisionTypeNameAr: string | nullable
- transmisionTypeNameEn: string | nullable
- drivetrain: integer(int32) | nullable
- drivetrainNameAr: string | nullable
- drivetrainNameEn: string | nullable
- cylenders: integer(int32) | nullable
- fuelType: integer(int32) | nullable
- fuelTypeNameAr: string | nullable
- fuelTypeNameEn: string | nullable
- manufactureCountryId: integer(int32) | nullable
- manufactureCountryNameAr: string | nullable
- manufactureCountryNameEn: string | nullable
- enginNumber: string | nullable
- descriptionAr: string | nullable
- descriptionEn: string | nullable
- createdAt: string(date-time)
- createdBy: string | nullable
- isAvailable: boolean
- features: array[CarFeatureResponseDto] | nullable
-   features[].id: integer(int32)
-   features[].nameAr: string | nullable
-   features[].nameEn: string | nullable
-   features[].isAvailable: boolean
- colors: array[CarCarColorResponseDto] | nullable
-   colors[].carId: integer(int32)
-   colors[].colorId: integer(int32)
-   colors[].colorNameAr: string | nullable
-   colors[].colorNameEn: string | nullable
-   colors[].colorCode: string | nullable
-   colors[].colorStatus: integer(int32)
-   colors[].colorStatusDetailCode: string | nullable
-   colors[].colorStatusNameAr: string | nullable
-   colors[].colorStatusNameEn: string | nullable
-   colors[].stockQuantity: integer(int32) | nullable
-   colors[].colorImageUrl: string | nullable
-   colors[].pricingPerColor: number(double) | nullable
-   colors[].pricePefore: number(double) | nullable
-   colors[].vatAmount: number(double) | nullable
-   colors[].discount: number(double) | nullable
-   colors[].discountType: integer(int32) | nullable
-   colors[].totalPrice: number(double) | nullable
-   colors[].isAvailable: boolean
- extraDetails: array[CarExtraDetailApiDto] | nullable
-   extraDetails[].id: integer(int32)
-   extraDetails[].nameAr: string | nullable
-   extraDetails[].nameEn: string | nullable
-   extraDetails[].descriptionEn: string | nullable
-   extraDetails[].descriptionAr: string | nullable
-   extraDetails[].carExtraDetailsType: integer(int32)
-   extraDetails[].carExtraDetailsTypeNameAr: string | nullable
-   extraDetails[].carExtraDetailsTypeNameEn: string | nullable
-   extraDetails[].createdBy: string | nullable
-   extraDetails[].isAvailable: boolean
-   extraDetails[].carId: integer(int32)
- galleryImages: array[CarGalleryImageApiDto] | nullable
-   galleryImages[].id: integer(int32)
-   galleryImages[].carId: integer(int32)
-   galleryImages[].imageUrl: string | nullable
-   galleryImages[].imageType: integer(int32) | nullable
-   galleryImages[].imageTypeNameAr: string | nullable
-   galleryImages[].imageTypeNameEn: string | nullable
-   galleryImages[].isPrimary: boolean
-   galleryImages[].createdBy: string | nullable
-   galleryImages[].isAvailable: boolean

**Example**

```json
{
  "id": 1,
  "nameAr": "string",
  "nameEn": "string",
  "modelId": 1,
  "modelNameAr": "string",
  "modelNameEn": "string",
  "brandId": 1,
  "brandNameAr": "string",
  "brandNameEn": "string",
  "typeId": 1,
  "typeNameAr": "string",
  "typeNameEn": "string",
  "branchId": 1,
  "branchNameAr": "string",
  "branchNameEn": "string",
  "year": 1,
  "mileage": 1,
  "vat": 0.0,
  "conditionId": 1,
  "conditionNameAr": "string",
  "conditionNameEn": "string",
  "seatingCapacity": 1,
  "weelSizeInch": "string",
  "fuelTankCapacityLiter": 0.0,
  "trimLevel": 1,
  "trimLevelNameAr": "string",
  "trimLevelNameEn": "string",
  "vehicleClass": 1,
  "vehicleClassNameAr": "string",
  "vehicleClassNameEn": "string",
  "plateNumberAr": "string",
  "plateNumberEn": "string",
  "transmisionType": 1,
  "transmisionTypeNameAr": "string",
  "transmisionTypeNameEn": "string",
  "drivetrain": 1,
  "drivetrainNameAr": "string",
  "drivetrainNameEn": "string",
  "cylenders": 1,
  "fuelType": 1,
  "fuelTypeNameAr": "string",
  "fuelTypeNameEn": "string",
  "manufactureCountryId": 1,
  "manufactureCountryNameAr": "string",
  "manufactureCountryNameEn": "string",
  "enginNumber": "string",
  "descriptionAr": "string",
  "descriptionEn": "string",
  "createdAt": "2026-05-11T00:00:00Z",
  "createdBy": "string",
  "isAvailable": true,
  "features": [
    {
      "id": 1,
      "nameAr": "string",
      "nameEn": "string",
      "isAvailable": true
    }
  ],
  "colors": [
    {
      "carId": 1,
      "colorId": 1,
      "colorNameAr": "string",
      "colorNameEn": "string",
      "colorCode": "string",
      "colorStatus": 1,
      "colorStatusDetailCode": "string",
      "colorStatusNameAr": "string",
      "colorStatusNameEn": "string",
      "stockQuantity": 1,
      "colorImageUrl": "string",
      "pricingPerColor": 0.0,
      "pricePefore": 0.0,
      "vatAmount": 0.0,
      "discount": 0.0,
      "discountType": 1,
      "totalPrice": 0.0,
      "isAvailable": true
    }
  ],
  "extraDetails": [
    {
      "id": 1,
      "nameAr": "string",
      "nameEn": "string",
      "descriptionEn": "string",
      "descriptionAr": "string",
      "carExtraDetailsType": 1,
      "carExtraDetailsTypeNameAr": "string",
      "carExtraDetailsTypeNameEn": "string",
      "createdBy": "string",
      "isAvailable": true,
      "carId": 1
    }
  ],
  "galleryImages": [
    {
      "id": 1,
      "carId": 1,
      "imageUrl": "string",
      "imageType": 1,
      "imageTypeNameAr": "string",
      "imageTypeNameEn": "string",
      "isPrimary": true,
      "createdBy": "string",
      "isAvailable": true
    }
  ]
}
```

### `CarCarColorResponseDto`
**Fields**

- carId: integer(int32)
- colorId: integer(int32)
- colorNameAr: string | nullable
- colorNameEn: string | nullable
- colorCode: string | nullable
- colorStatus: integer(int32)
- colorStatusDetailCode: string | nullable
- colorStatusNameAr: string | nullable
- colorStatusNameEn: string | nullable
- stockQuantity: integer(int32) | nullable
- colorImageUrl: string | nullable
- pricingPerColor: number(double) | nullable
- pricePefore: number(double) | nullable
- vatAmount: number(double) | nullable
- discount: number(double) | nullable
- discountType: integer(int32) | nullable
- totalPrice: number(double) | nullable
- isAvailable: boolean

**Example**

```json
{
  "carId": 1,
  "colorId": 1,
  "colorNameAr": "string",
  "colorNameEn": "string",
  "colorCode": "string",
  "colorStatus": 1,
  "colorStatusDetailCode": "string",
  "colorStatusNameAr": "string",
  "colorStatusNameEn": "string",
  "stockQuantity": 1,
  "colorImageUrl": "string",
  "pricingPerColor": 0.0,
  "pricePefore": 0.0,
  "vatAmount": 0.0,
  "discount": 0.0,
  "discountType": 1,
  "totalPrice": 0.0,
  "isAvailable": true
}
```

### `CarExtraDetailApiDto`
**Fields**

- id: integer(int32)
- nameAr: string | nullable
- nameEn: string | nullable
- descriptionEn: string | nullable
- descriptionAr: string | nullable
- carExtraDetailsType: integer(int32)
- carExtraDetailsTypeNameAr: string | nullable
- carExtraDetailsTypeNameEn: string | nullable
- createdBy: string | nullable
- isAvailable: boolean
- carId: integer(int32)

**Example**

```json
{
  "id": 1,
  "nameAr": "string",
  "nameEn": "string",
  "descriptionEn": "string",
  "descriptionAr": "string",
  "carExtraDetailsType": 1,
  "carExtraDetailsTypeNameAr": "string",
  "carExtraDetailsTypeNameEn": "string",
  "createdBy": "string",
  "isAvailable": true,
  "carId": 1
}
```

### `CarFeatureResponseDto`
**Fields**

- id: integer(int32)
- nameAr: string | nullable
- nameEn: string | nullable
- isAvailable: boolean

**Example**

```json
{
  "id": 1,
  "nameAr": "string",
  "nameEn": "string",
  "isAvailable": true
}
```

### `CarGalleryImageApiDto`
**Fields**

- id: integer(int32)
- carId: integer(int32)
- imageUrl: string | nullable
- imageType: integer(int32) | nullable
- imageTypeNameAr: string | nullable
- imageTypeNameEn: string | nullable
- isPrimary: boolean
- createdBy: string | nullable
- isAvailable: boolean

**Example**

```json
{
  "id": 1,
  "carId": 1,
  "imageUrl": "string",
  "imageType": 1,
  "imageTypeNameAr": "string",
  "imageTypeNameEn": "string",
  "isPrimary": true,
  "createdBy": "string",
  "isAvailable": true
}
```

### `CarModelByBrandResponseDto`
**Fields**

- id: integer(int32)
- nameAr: string | nullable
- nameEn: string | nullable
- brandId: integer(int32)
- isAvailable: boolean
- createdAt: string(date-time)

**Example**

```json
{
  "id": 1,
  "nameAr": "string",
  "nameEn": "string",
  "brandId": 1,
  "isAvailable": true,
  "createdAt": "2026-05-11T00:00:00Z"
}
```

### `ContactSalesOfficerResponseDto`
**Fields**

- id: integer(int32)
- contactValue: string | nullable
- contactType: integer(int32)
- contactTypeNameAr: string | nullable
- contactTypeNameEn: string | nullable
- contactIconUrl: string | nullable
- createdBy: string | nullable
- isAvailable: boolean
- createdAt: string(date-time)
- branchId: integer(int32)

**Example**

```json
{
  "id": 1,
  "contactValue": "string",
  "contactType": 1,
  "contactTypeNameAr": "string",
  "contactTypeNameEn": "string",
  "contactIconUrl": "string",
  "createdBy": "string",
  "isAvailable": true,
  "createdAt": "2026-05-11T00:00:00Z",
  "branchId": 1
}
```

### `ContactUsResponseDto`
**Fields**

- id: integer(int32)
- contactValue: string | nullable
- contactType: integer(int32)
- contactTypeNameAr: string | nullable
- contactTypeNameEn: string | nullable
- contactIconUrl: string | nullable
- messageAr: string | nullable
- messageEn: string | nullable
- createdBy: string | nullable
- isAvailable: boolean
- createdAt: string(date-time)

**Example**

```json
{
  "id": 1,
  "contactValue": "string",
  "contactType": 1,
  "contactTypeNameAr": "string",
  "contactTypeNameEn": "string",
  "contactIconUrl": "string",
  "messageAr": "string",
  "messageEn": "string",
  "createdBy": "string",
  "isAvailable": true,
  "createdAt": "2026-05-11T00:00:00Z"
}
```

### `CreateMyFavoriteRequest`
**Fields**

- carId: integer(int32)
- notes: string | nullable
- priority: integer(int32)

**Example**

```json
{
  "carId": 1,
  "notes": "string",
  "priority": 1
}
```

### `CreateRequestDto`
**Fields**

- userId: string(uuid) | nullable
- vehicleOwnerType: integer(int32)
- name: string | nullable
- email: string | nullable
- mobileNo: string | nullable
- carId: integer(int32)
- colorId: integer(int32)
- paymentMethod: integer(int32)
- regionId: integer(int32)
- cityId: integer(int32)
- notes: string | nullable

**Example**

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "vehicleOwnerType": 1,
  "name": "string",
  "email": "string",
  "mobileNo": "string",
  "carId": 1,
  "colorId": 1,
  "paymentMethod": 1,
  "regionId": 1,
  "cityId": 1,
  "notes": "string"
}
```

### `FAQResponseDto`
**Fields**

- id: integer(int32)
- titleAr: string | nullable
- titleEn: string | nullable
- descriptionAr: string | nullable
- descriptionEn: string | nullable
- order: integer(int32)
- isAvailable: boolean

**Example**

```json
{
  "id": 1,
  "titleAr": "string",
  "titleEn": "string",
  "descriptionAr": "string",
  "descriptionEn": "string",
  "order": 1,
  "isAvailable": true
}
```

### `ForgotPasswordRequest`
**Fields**

- userNameOrEmail: string | nullable

**Example**

```json
{
  "userNameOrEmail": "string"
}
```

### `LoginRequest`
**Fields**

- userName: string | nullable
- password: string | nullable
- rememberMe: boolean

**Example**

```json
{
  "userName": "string",
  "password": "string",
  "rememberMe": true
}
```

### `MemberServiceResponseDto`
**Fields**

- id: integer(int32)
- nameAr: string | nullable
- nameEn: string | nullable
- descriptionAr: string | nullable
- descriptionEn: string | nullable
- imageUrl: string | nullable
- isAvailable: boolean

**Example**

```json
{
  "id": 1,
  "nameAr": "string",
  "nameEn": "string",
  "descriptionAr": "string",
  "descriptionEn": "string",
  "imageUrl": "string",
  "isAvailable": true
}
```

### `OfferResponseDto`
**Fields**

- id: integer(int32)
- offerImageUrl: string | nullable
- offerNameAr: string | nullable
- offerNameEn: string | nullable
- descriptionAr: string | nullable
- descriptionEn: string | nullable
- expiredAt: string(date-time) | nullable
- isAvailable: boolean

**Example**

```json
{
  "id": 1,
  "offerImageUrl": "string",
  "offerNameAr": "string",
  "offerNameEn": "string",
  "descriptionAr": "string",
  "descriptionEn": "string",
  "expiredAt": "2026-05-11T00:00:00Z",
  "isAvailable": true
}
```

### `PackageResponseDto`
**Fields**

- id: integer(int32)
- nameAr: string | nullable
- nameEn: string | nullable
- imageUrl: string | nullable
- createdBy: string | nullable
- isAvailable: boolean

**Example**

```json
{
  "id": 1,
  "nameAr": "string",
  "nameEn": "string",
  "imageUrl": "string",
  "createdBy": "string",
  "isAvailable": true
}
```

### `ResetPasswordRequest`
**Fields**

- userNameOrEmail: string | nullable
- token: string | nullable
- newPassword: string | nullable

**Example**

```json
{
  "userNameOrEmail": "string",
  "token": "string",
  "newPassword": "string"
}
```

### `ServicesResponseDto`
**Fields**

- id: integer(int32)
- nameAr: string | nullable
- nameEn: string | nullable
- descriptionAr: string | nullable
- descriptionEn: string | nullable
- discount: number(double)
- isPercentage: boolean
- serviceImageUrl: string | nullable
- isAvailable: boolean

**Example**

```json
{
  "id": 1,
  "nameAr": "string",
  "nameEn": "string",
  "descriptionAr": "string",
  "descriptionEn": "string",
  "discount": 0.0,
  "isPercentage": true,
  "serviceImageUrl": "string",
  "isAvailable": true
}
```

### `UserFavoriteAdminResponseDto`
**Fields**

- userId: string(uuid)
- carId: integer(int32)
- carNameAr: string | nullable
- carNameEn: string | nullable
- modelId: integer(int32) | nullable
- modelNameAr: string | nullable
- modelNameEn: string | nullable
- brandId: integer(int32) | nullable
- brandNameAr: string | nullable
- brandNameEn: string | nullable
- notes: string | nullable
- priority: integer(int32)
- createdAt: string(date-time)

**Example**

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "carId": 1,
  "carNameAr": "string",
  "carNameEn": "string",
  "modelId": 1,
  "modelNameAr": "string",
  "modelNameEn": "string",
  "brandId": 1,
  "brandNameAr": "string",
  "brandNameEn": "string",
  "notes": "string",
  "priority": 1,
  "createdAt": "2026-05-11T00:00:00Z"
}
```

