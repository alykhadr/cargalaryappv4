import { icons, images } from "../constants";

export const userAddresses = [
    {
        id: "1",
        name: "Home",
        address: "King Fahd Road, Al Olaya, Riyadh 12212",
    },
    {
        id: "2",
        name: "Office",
        address: "Prince Mohammed bin Salman Rd, Al Nakheel, Riyadh",
    },
    {
        id: "3",
        name: "Mall of Arabia",
        address: "King Abdullah Rd, Al Rawdah, Jeddah 23434",
    },
    {
        id: "4",
        name: "Riyadh Park",
        address: "King Fahd Rd, Al Nakheel, Riyadh 12382",
    },
    {
        id: "5",
        name: "Corniche",
        address: "Al Corniche Rd, Al Shati, Jeddah 21511"
    },
    {
        id: "6",
        name: "Tahlia Street",
        address: "Tahlia St, Al Sulaimaniyah, Riyadh 12241"
    },
    {
        id: "7",
        name: "Al Rashid Mall",
        address: "King Fahd Rd, Al Khobar 31952, Eastern Province"
    }
];

export const faqKeywords = [
    {
        id: "1",
        name: "General"
    },
    {
        id: "2",
        name: "Account"
    },
    {
        id: "3",
        name: "Security"
    },
    {
        id: "4",
        name: "Listing"
    },
    {
        id: "5",
        name: "Payment"
    }
];

export const faqs = [
    {
        question: 'How do I list a car on the app?',
        answer: 'To list a car, simply go to the "Sell Your Car" section, provide all the necessary details about your car, upload clear photos, and publish your listing for potential buyers to see.',
        type: "General"
    },
    {
        question: 'Can I view car details, such as specifications and availability?',
        answer: 'Yes, you can view detailed car information including specifications, availability, and seller reviews. Simply navigate to the car listing page within the app.',
        type: "General"
    },
    {
        question: 'What should I do if I need to cancel or modify a listing?',
        answer: 'To cancel or modify a listing, go to the "My Listings" section, find your listing, and follow the provided options to make changes.',
        type: "Account"
    },
    {
        question: 'How can I find cars from specific makes or models?',
        answer: 'You can use the app’s search filters to find cars from specific makes or models. Filter results by categories such as sedans, SUVs, or trucks.',
        type: "Listing"
    },
    {
        question: 'Is there a way to make payments for car purchases within the app?',
        answer: 'Yes, you can securely make payments for car purchases using various payment methods available in the app, including bank transfers and escrow services.',
        type: "Payment"
    },
    {
        question: 'Are my personal details and car information kept secure?',
        answer: 'Yes, we prioritize the security and confidentiality of your personal details and car information. Our app complies with strict privacy and data protection standards.',
        type: "Security"
    },
    {
        question: 'Can I request assistance with special car requirements or preferences?',
        answer: "Yes, you can request assistance with special car requirements or preferences during the listing process. Simply specify your preferences, and we'll do our best to accommodate them.",
        type: "General"
    },
    {
        question: 'How can I provide feedback or review my car selling/buying experience?',
        answer: 'After selling or buying a car, you can provide feedback and review your experience through the app’s rating and review system. Your feedback helps us improve our services for future transactions.',
        type: "General"
    },
    {
        question: 'Is customer support available through this app?',
        answer: 'While we facilitate car transactions, our app is not for customer support. For assistance, please contact our support team through the designated channels provided in the app.',
        type: "General"
    },
];

export const friends = [
    {
        id: "1",
        name: "Tynisa Obey",
        phoneNumber: "+1-300-400-0135",
        avatar: images.user1,
    },
    {
        id: "2",
        name: "Florencio Dorance",
        phoneNumber: "+1-309-900-0135",
        avatar: images.user2,
    },
    {
        id: "3",
        name: "Chantal Shelburne",
        phoneNumber: "+1-400-100-1009",
        avatar: images.user3,
    },
    {
        id: "4",
        name: "Maryland Winkles",
        phoneNumber: "+1-970-200-4550",
        avatar: images.user4,
    },
    {
        id: "5",
        name: "Rodolfo Goode",
        phoneNumber: "+1-100-200-9800",
        avatar: images.user5,
    },
    {
        id: "6",
        name: "Benny Spanbauer",
        phoneNumber: "+1-780-200-9800",
        avatar: images.user6,
    },
    {
        id: "7",
        name: "Tyra Dillon",
        phoneNumber: "+1-943-230-9899",
        avatar: images.user7,
    },
    {
        id: "8",
        name: "Jamel Eusobio",
        phoneNumber: "+1-900-234-9899",
        avatar: images.user8,
    },
    {
        id: "9",
        name: "Pedro Haurad",
        phoneNumber: "+1-240-234-9899",
        avatar: images.user9
    },
    {
        id: "10",
        name: "Clinton Mcclure",
        phoneNumber: "+1-500-234-4555",
        avatar: images.user10
    },
];

export const transactionHistory = [
    {
        id: "1",
        image: images.user1,
        name: "Daniel Austin",
        date: "Dec 20, 2024 | 10:00 AM",
        type: "Purchase Expense",
        amount: "$14"
    },
    {
        id: "2",
        image: images.user2,
        name: "Top Up Wallet",
        date: "Dec 16, 2024 | 13:34 PM",
        type: "Top Up",
        amount: "$80"
    },
    {
        id: "3",
        image: images.user3,
        name: "Sarah Wilson",
        date: "Dec 14, 2024 | 11:39 AM",
        type: "Purchase Expense",
        amount: "$32"
    },
    {
        id: "4",
        image: images.user2,
        name: "Daniel Austion",
        date: "Dec 10, 2024 | 09:32 AM",
        type: "Top Up",
        amount: "$112"
    },
    {
        id: "5",
        image: images.user5,
        name: "Benny Spanbauleur",
        date: "Dec 09, 2024 | 10:08 AM",
        type: "Purchase Expense",
        amount: "$43"
    },
    {
        id: "6",
        image: images.user6,
        name: "Roselle Dorrence",
        date: "Dec 08, 2024 | 09:12 AM",
        type: "Purchase Expense",
        amount: "$22"
    },
    {
        id: "7",
        image: images.user2,
        name: "Daniel Austion",
        date: "Dec 08, 2024 | 16:28 PM",
        type: "Top Up",
        amount: "$200"
    },
    {
        id: "8",
        image: images.user2,
        name: "Daniel Austion",
        date: "Dec 07, 2024 | 15:12 PM",
        type: "Top Up",
        amount: "$120"
    },
    {
        id: "9",
        image: images.user2,
        name: "Daniel Austion",
        date: "Dec 07, 2024 | 22:12 PM",
        type: "Top Up",
        amount: "$20"
    },
    {
        id: "10",
        image: images.user8,
        name: "Lucky Luck",
        date: "Dec 06, 2024 | 10:08 AM",
        type: "Purchase Expense",
        amount: "$12"
    },
    {
        id: "11",
        image: images.user2,
        name: "Jennifer Lucie",
        date: "Dec 03, 2024 | 11:48 AM",
        type: "Top Up",
        amount: "$45"
    }
];

export const messsagesData = [
    {
        id: "1",
        fullName: "Jhon Smith",
        userImg: images.user1,
        lastSeen: "2023-11-16T04:52:06.501Z",
        lastMessage: 'I love you. see you soon baby',
        messageInQueue: 2,
        lastMessageTime: "12:25 PM",
        isOnline: true,
    },
    {
        id: "2",
        fullName: "Anuska Sharma",
        userImg: images.user2,
        lastSeen: "2023-11-18T04:52:06.501Z",
        lastMessage: 'I Know. you are so busy man.',
        messageInQueue: 0,
        lastMessageTime: "12:15 PM",
        isOnline: false
    },
    {
        id: "3",
        fullName: "Virat Kohili",
        userImg: images.user3,
        lastSeen: "2023-11-20T04:52:06.501Z",
        lastMessage: 'Ok, see u soon',
        messageInQueue: 0,
        lastMessageTime: "09:12 PM",
        isOnline: true
    },
    {
        id: "4",
        fullName: "Shikhor Dhaon",
        userImg: images.user4,
        lastSeen: "2023-11-18T04:52:06.501Z",
        lastMessage: 'Great! Do you Love it.',
        messageInQueue: 0,
        lastMessageTime: "04:12 PM",
        isOnline: true
    },
    {
        id: "5",
        fullName: "Shakib Hasan",
        userImg: images.user5,
        lastSeen: "2023-11-21T04:52:06.501Z",
        lastMessage: 'Thank you !',
        messageInQueue: 2,
        lastMessageTime: "10:30 AM",
        isOnline: true
    },
    {
        id: "6",
        fullName: "Jacksoon",
        userImg: images.user6,
        lastSeen: "2023-11-20T04:52:06.501Z",
        lastMessage: 'Do you want to go out dinner',
        messageInQueue: 3,
        lastMessageTime: "10:05 PM",
        isOnline: false
    },
    {
        id: "7",
        fullName: "Tom Jerry",
        userImg: images.user7,
        lastSeen: "2023-11-20T04:52:06.501Z",
        lastMessage: 'Do you want to go out dinner',
        messageInQueue: 2,
        lastMessageTime: "11:05 PM",
        isOnline: true
    },
    {
        id: "8",
        fullName: "Lucky Luck",
        userImg: images.user8,
        lastSeen: "2023-11-20T04:52:06.501Z",
        lastMessage: 'Can you share the design with me?',
        messageInQueue: 2,
        lastMessageTime: "09:11 PM",
        isOnline: true
    },
    {
        id: "9",
        fullName: "Nate Jack",
        userImg: images.user9,
        lastSeen: "2023-11-20T04:52:06.501Z",
        lastMessage: 'Tell me what you want?',
        messageInQueue: 0,
        lastMessageTime: "06:43 PM",
        isOnline: true
    }
];

export const callData = [
    {
        id: "1",
        fullName: "Roselle Erhman",
        userImg: images.user10,
        status: "Incoming",
        date: "Dec 19, 2024"
    },
    {
        id: "2",
        fullName: "Willard Purnell",
        userImg: images.user9,
        status: "Outgoing",
        date: "Dec 17, 2024"
    },
    {
        id: "3",
        fullName: "Charlotte Hanlin",
        userImg: images.user8,
        status: "Missed",
        date: "Dec 16, 2024"
    },
    {
        id: "4",
        fullName: "Merlin Kevin",
        userImg: images.user7,
        status: "Missed",
        date: "Dec 16, 2024"
    },
    {
        id: "5",
        fullName: "Lavern Laboy",
        userImg: images.user6,
        status: "Outgoing",
        date: "Dec 16, 2024"
    },
    {
        id: "6",
        fullName: "Phyllis Godley",
        userImg: images.user5,
        status: "Incoming",
        date: "Dec 15, 2024"
    },
    {
        id: "7",
        fullName: "Tyra Dillon",
        userImg: images.user4,
        status: "Outgoing",
        date: "Dec 15, 2024"
    },
    {
        id: "8",
        fullName: "Marci Center",
        userImg: images.user3,
        status: "Missed",
        date: "Dec 15, 2024"
    },
    {
        id: "9",
        fullName: "Clinton Mccure",
        userImg: images.user2,
        status: "Outgoing",
        date: "Dec 15, 2024"
    },
];

export const banners = [
    {
        id: 1,
        discount: 'NEW',
        discountName: "Land Cruiser 2025",
        bottomTitle: 'Now available — GXR & VXR trims',
        bottomSubtitle: 'Delivery across KSA within 3 days',
        imageKey: 'https://cdn.syarah.com/photos-thumbs/online-v1/0x426/online/posts/302867/orignal-1779625879-919_cut.jpg?v=3',
        brandColor: '#EB0A1E'
    },
    {
        id: 2,
        discount: '15%',
        discountName: "BMW Summer Deals",
        bottomTitle: 'X5, X7 & 7 Series — limited stock',
        bottomSubtitle: 'Finance from 1,890 SAR / month',
        imageKey: 'https://cdn.syarah.com/photos-thumbs/online-v1/0x426/online/posts/301247/orignal-301247-1-1778395884.jpg?v=3',
        brandColor: '#1A96F0'
    },
    {
        id: 3,
        discount: 'VIP',
        discountName: "Mercedes S-Class",
        bottomTitle: 'S580 & GLE 450 — exclusive offers',
        bottomSubtitle: 'Al Rajhi & Riyad Bank financing',
        imageKey: 'https://cdn.syarah.com/photos-thumbs/online-v1/0x426/online/posts/301891/orignal-1779625948-99_cut.jpg?v=3',
        brandColor: '#2C3E50'
    }
];

export const categories = [
    {
        id: "0",
        name: "All",
        icon: icons.category,
        iconColor: "rgba(51, 94, 247, 1)",
        backgroundColor: "rgba(51, 94, 247, .12)",
        onPress: null
    },
    {
        id: "4",
        name: "Toyota",
        icon: icons.toyota,
        iconColor: "rgba(235, 10, 30, 1)",
        backgroundColor: "rgba(235, 10, 30, .12)",
        onPress: "categorytoyota"
    },
    {
        id: "3",
        name: "BMW",
        icon: icons.bmw,
        iconColor: "rgba(26, 150, 240, 1)",
        backgroundColor: "rgba(26, 150, 240, .12)",
        onPress: "categorybmw"
    },
    {
        id: "1",
        name: "Mercedes",
        icon: icons.mercedes,
        iconColor: "rgba(44, 62, 80, 1)",
        backgroundColor: "rgba(44, 62, 80, .12)",
        onPress: "categorymercedes"
    },
    {
        id: "5",
        name: "Volvo",
        icon: icons.volvo,
        iconColor: "rgba(0, 68, 124, 1)",
        backgroundColor: "rgba(0, 68, 124, .12)",
        onPress: "categoryvolvo"
    },
    {
        id: "2",
        name: "Tesla",
        icon: icons.tesla,
        iconColor: "rgba(30, 30, 30, 1)",
        backgroundColor: "rgba(30, 30, 30, .10)",
        onPress: "categorytesla"
    },
    {
        id: "7",
        name: "Honda",
        icon: icons.honda,
        iconColor: "rgba(200, 16, 46, 1)",
        backgroundColor: "rgba(200, 16, 46, .12)",
        onPress: "categoryhonda"
    },
    {
        id: "6",
        name: "Bugatti",
        icon: icons.bugatti,
        iconColor: "rgba(74, 175, 87, 1)",
        backgroundColor: "rgba(74, 175, 87, .12)",
        onPress: "categorybugatti"
    },
    {
        id: "8",
        name: "Others",
        icon: icons.more2,
        iconColor: "rgba(114, 16, 255, 1)",
        backgroundColor: "rgba(114, 16, 255, .12)",
        onPress: null
    }
];

export const ratings = [
    {
        id: "1",
        title: "All"
    },
    {
        id: "6",
        title: "5"
    },
    {
        id: "5",
        title: "4"
    },
    {
        id: "4",
        title: "3"
    },
    {
        id: "3",
        title: "2"
    },
    {
        id: "2",
        title: "1"
    }
];

export const sorts = [
    {
        id: "1",
        name: "Popular"
    },
    {
        id: "2",
        name: "Most Recent"
    },
    {
        id: "3",
        name: "Price High"
    },
    {
        id: "4",
        name: "Price Low"
    },
    {
        id: "5",
        name: "Most Rated"
    },
];

export const carReviews = [
    {
        id: "1",
        avatar: images.user1,
        name: "John Smith",
        description: "This car was simply amazing! The powerful engine and smooth handling made driving effortless. Highly recommended! 😍",
        rating: 4.8,
        avgRating: 5,
        date: "2025-03-28T12:00:00.000Z",
        numLikes: 320
    },
    {
        id: "2",
        avatar: images.user2,
        name: "Emily Davis",
        description: "I thoroughly enjoyed this car. The comfort and performance were exceptional. Definitely a top choice for any driver!",
        rating: 4.7,
        avgRating: 5,
        date: "2025-03-28T12:00:00.000Z",
        numLikes: 95
    },
    {
        id: "3",
        avatar: images.user3,
        name: "Michael Rodriguez",
        description: "This car exceeded my expectations! The build quality and technology features were remarkable. Will be recommending it to friends!",
        rating: 4.9,
        avgRating: 5,
        date: "2025-03-29T12:00:00.000Z",
        numLikes: 210
    },
    {
        id: "4",
        avatar: images.user4,
        name: "Sarah Brown",
        description: "I had a wonderful experience with this car. The design and functionality were outstanding, making it a joy to drive. Highly recommend!",
        rating: 4.5,
        avgRating: 5,
        date: "2025-03-29T12:00:00.000Z",
        numLikes: 150
    },
    {
        id: "5",
        avatar: images.user5,
        name: "David Wilson",
        description: "Absolutely fantastic! This car exceeded my expectations with its performance and reliability. It's a must-have for any driver!",
        rating: 3.8,
        avgRating: 4,
        date: "2025-02-31T12:00:00.000Z",
        numLikes: 500
    },
    {
        id: "6",
        avatar: images.user6,
        name: "Luca Dalasias",
        description: "This car exceeded my expectations! The build quality and performance were remarkable. Will be recommending it to friends!",
        rating: 4.8,
        avgRating: 5,
        date: "2025-02-29T12:00:00.000Z",
        numLikes: 210
    },
    {
        id: "7",
        avatar: images.user7,
        name: "Sophia Johnson",
        description: "I'm impressed by this car! The comfort and technology features make it a great choice for daily driving.",
        rating: 4.6,
        avgRating: 5,
        date: "2025-04-15T12:00:00.000Z",
        numLikes: 180
    },
    {
        id: "8",
        avatar: images.user8,
        name: "Daniel White",
        description: "This car is a game-changer! It offers great performance and safety features, making driving enjoyable and secure. Highly recommend it to everyone!",
        rating: 4.9,
        avgRating: 5,
        date: "2025-04-20T12:00:00.000Z",
        numLikes: 250
    },
    {
        id: "9",
        avatar: images.user9,
        name: "Olivia Martinez",
        description: "I'm in love with this car! It's stylish, comfortable, and offers a smooth driving experience. Definitely worth every penny!",
        rating: 5.0,
        avgRating: 5,
        date: "2025-04-22T12:00:00.000Z",
        numLikes: 380
    },
];

export const myCart = [
    {
        id: "1",
        name: "Honda Civic",
        image: images.honda1,
        price: "22,000.00",
        numReviews: 143,
        rating: 4.5,
        quantity: 1300,
        numSolds: 9373,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#949494",
    },
    {
        id: "2",
        name: "BMW X3 SUV",
        image: images.bmw3,
        price: "55,000.00",
        numReviews: 205,
        rating: 4.8,
        quantity: 300,
        numSolds: 1689,
        navigate: "bmwdetails",
        categoryId: "3",
        color: "#101010",
    },
    {
        id: "3",
        name: "Honda Accord",
        image: images.honda2,
        price: "24,000.00",
        numReviews: 98,
        rating: 4.2,
        quantity: 500,
        numSolds: 562,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#949494",
    },
    {
        id: "4",
        name: "Honda CR-V",
        image: images.honda3,
        price: "28,000.00",
        numReviews: 205,
        rating: 4.8,
        quantity: 300,
        numSolds: 1689,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#949494",
    },
    {
        id: "5",
        name: "Mercedes-Benz GLC SUV",
        image: images.mercedes4,
        price: "60,000.00",
        numReviews: 72,
        rating: 4.0,
        quantity: 700,
        numSolds: 423,
        navigate: "mercedesdetails",
        categoryId: "1",
        color: "#949494",
    },
    {
        id: "6",
        name: "Tesla Roadster",
        image: images.tesla5,
        price: "200,000.00",
        numReviews: 120,
        rating: 4.3,
        quantity: 400,
        numSolds: 987,
        navigate: "tesladetails",
        categoryId: "2",
        color: "#949494",
    },
    {
        id: "7",
        name: "Toyota Highlander",
        image: images.toyota4,
        price: "40,000.00",
        numReviews: 72,
        rating: 4.0,
        quantity: 700,
        numSolds: 423,
        navigate: "toyotadetails",
        categoryId: "4",
        color: "#949494",
    },
    {
        id: "8",
        name: "Tesla Cybertruck",
        image: images.tesla6,
        price: "70,000.00",
        numReviews: 64,
        rating: 4.1,
        quantity: 250,
        numSolds: 325,
        navigate: "tesladetails",
        categoryId: "2",
        color: "#949494",
    },
    {
        id: "9",
        name: "Mercedes-Benz S-Class Sedan",
        image: images.mercedes5,
        price: "100,000.00",
        numReviews: 120,
        rating: 4.3,
        quantity: 400,
        numSolds: 987,
        navigate: "mercedesdetails",
        categoryId: "1",
        color: "#949494",
    },
    {
        id: "10",
        name: "Toyota Tacoma",
        image: images.toyota5,
        price: "35,000.00",
        numReviews: 120,
        rating: 4.3,
        quantity: 400,
        numSolds: 987,
        navigate: "toyotadetails",
        categoryId: "4",
        color: "#949494",
    },
    {
        id: "11",
        name: "Mercedes-Benz GLA SUV",
        image: images.mercedes6,
        price: "40,000.00",
        numReviews: 64,
        rating: 4.1,
        quantity: 250,
        numSolds: 325,
        navigate: "mercedesdetails",
        categoryId: "1",
        color: "#949494",
    },
    {
        id: "12",
        name: "BMW 7 Series Sedan",
        image: images.bmw5,
        price: "85,000.00",
        numReviews: 120,
        rating: 4.3,
        quantity: 400,
        numSolds: 987,
        navigate: "bmwdetails",
        categoryId: "3",
        color: "#949494",
    },
    {
        id: "13",
        name: "Bugatti Chiron",
        image: images.bugatti1,
        price: "3,000,000.00",
        numReviews: 143,
        rating: 4.5,
        quantity: 1300,
        numSolds: 9373,
        navigate: "bugattidetails",
        categoryId: "6",
        color: "#949494",
    },
    {
        id: "14",
        name: "Bugatti Veyron",
        image: images.bugatti2,
        price: "2,000,000.00",
        numReviews: 98,
        rating: 4.2,
        quantity: 500,
        numSolds: 562,
        navigate: "bugattidetails",
        categoryId: "6",
        color: "#949494",
    },
    {
        id: "15",
        name: "BMW X6 SUV",
        image: images.bmw6,
        price: "75,000.00",
        numReviews: 64,
        rating: 4.1,
        quantity: 250,
        numSolds: 325,
        navigate: "bmwdetails",
        categoryId: "3",
        color: "#949494",
    }
]

export const orderList = [
    {
        id: "1",
        name: "Honda Civic",
        image: images.honda1,
        price: "22,000.00",
        numReviews: 143,
        rating: 4.5,
        numSolds: 9373,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#101010",
        quantity: 1
    },
    {
        id: "2",
        name: "BMW X3 SUV",
        image: images.bmw3,
        price: "55,000.00",
        numReviews: 205,
        rating: 4.8,
        numSolds: 1689,
        navigate: "bmwdetails",
        categoryId: "3",
        color: "#7A5548",
        quantity: 2
    },
    {
        id: "3",
        name: "Honda Accord",
        image: images.honda2,
        price: "24,000.00",
        numReviews: 98,
        rating: 4.2,
        numSolds: 562,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#7A5548",
        quantity: 2
    }
]

export const ongoingOrders = [
    {
        id: "1",
        name: "Honda Civic",
        image: images.honda1,
        price: "22,000.00",
        numReviews: 143,
        rating: 4.5,
        numSolds: 9373,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#101010",
        quantity: 1,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "2",
        name: "BMW X3 SUV",
        image: images.bmw3,
        price: "55,000.00",
        numReviews: 205,
        rating: 4.8,
        numSolds: 1689,
        navigate: "bmwdetails",
        categoryId: "3",
        color: "#7A5548",
        quantity: 2,
        address: "910 Elm St, Hamlet",
        status: "Paid",
    },
    {
        id: "3",
        name: "Honda Accord",
        image: images.honda2,
        price: "24,000.00",
        numReviews: 98,
        rating: 4.2,
        numSolds: 562,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "4",
        name: "Honda CR-V",
        image: images.honda3,
        price: "28,000.00",
        numReviews: 205,
        rating: 4.8,
        numSolds: 1689,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#7A5548",
        quantity: 2,
        address: "321 Maple St, Suburbia",
        status: "Paid",
    },
    {
        id: "5",
        name: "Mercedes-Benz GLC SUV",
        image: images.mercedes4,
        price: "60,000.00",
        numReviews: 72,
        rating: 4.0,
        numSolds: 423,
        navigate: "mercedesdetails",
        categoryId: "1",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "6",
        name: "Tesla Roadster",
        image: images.tesla5,
        price: "200,000.00",
        numReviews: 120,
        rating: 4.3,
        numSolds: 987,
        navigate: "tesladetails",
        categoryId: "2",
        color: "#7A5548",
        quantity: 2,
        address: "567 Cedar St, Countryside",
        status: "Paid",
    },
    {
        id: "7",
        name: "Toyota Highlander",
        image: images.toyota4,
        price: "40,000.00",
        numReviews: 72,
        rating: 4.0,
        numSolds: 423,
        navigate: "toyotadetails",
        categoryId: "4",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "8",
        name: "Tesla Cybertruck",
        image: images.tesla6,
        price: "70,000.00",
        numReviews: 64,
        rating: 4.1,
        numSolds: 325,
        navigate: "tesladetails",
        categoryId: "2",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "9",
        name: "Mercedes-Benz S-Class Sedan",
        image: images.mercedes5,
        price: "100,000.00",
        numReviews: 120,
        rating: 4.3,
        numSolds: 987,
        navigate: "mercedesdetails",
        categoryId: "1",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "10",
        name: "Toyota Tacoma",
        image: images.toyota5,
        price: "35,000.00",
        numReviews: 120,
        rating: 4.3,
        numSolds: 987,
        navigate: "toyotadetails",
        categoryId: "4",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "11",
        name: "Mercedes-Benz GLA SUV",
        image: images.mercedes6,
        price: "40,000.00",
        numReviews: 64,
        rating: 4.1,
        numSolds: 325,
        navigate: "mercedesdetails",
        categoryId: "1",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "12",
        name: "BMW 7 Series Sedan",
        image: images.bmw5,
        price: "85,000.00",
        numReviews: 120,
        rating: 4.3,
        numSolds: 987,
        navigate: "bmwdetails",
        categoryId: "3",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "13",
        name: "Bugatti Chiron",
        image: images.bugatti1,
        price: "3,000,000.00",
        numReviews: 143,
        rating: 4.5,
        numSolds: 9373,
        navigate: "bugattidetails",
        categoryId: "6",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "14",
        name: "Bugatti Veyron",
        image: images.bugatti2,
        price: "2,000,000.00",
        numReviews: 98,
        rating: 4.2,
        numSolds: 562,
        navigate: "bugattidetails",
        categoryId: "6",
        color: "#7A5548",
        quantity: 2,
        address: "456 Oak St, Townsville",
        status: "Paid",
    },
    {
        id: "15",
        name: "BMW X6 SUV",
        image: images.bmw6,
        price: "75,000.00",
        numReviews: 64,
        rating: 4.1,
        numSolds: 325,
        navigate: "bmwdetails",
        categoryId: "3",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    }
]

export const completedOrders = [
    {
        id: "1",
        name: "Honda Civic",
        image: images.honda1,
        price: "22,000.00",
        numReviews: 143,
        rating: 4.5,
        numSolds: 9373,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#101010",
        quantity: 1,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "2",
        name: "BMW X3 SUV",
        image: images.bmw3,
        price: "55,000.00",
        numReviews: 205,
        rating: 4.8,
        numSolds: 1689,
        navigate: "bmwdetails",
        categoryId: "3",
        color: "#7A5548",
        quantity: 2,
        address: "910 Elm St, Hamlet",
        status: "Paid",
    },
    {
        id: "3",
        name: "Honda Accord",
        image: images.honda2,
        price: "24,000.00",
        numReviews: 98,
        rating: 4.2,
        numSolds: 562,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "4",
        name: "Honda CR-V",
        image: images.honda3,
        price: "28,000.00",
        numReviews: 205,
        rating: 4.8,
        numSolds: 1689,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#7A5548",
        quantity: 2,
        address: "321 Maple St, Suburbia",
        status: "Paid",
    },
    {
        id: "5",
        name: "Mercedes-Benz GLC SUV",
        image: images.mercedes4,
        price: "60,000.00",
        numReviews: 72,
        rating: 4.0,
        numSolds: 423,
        navigate: "mercedesdetails",
        categoryId: "1",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "6",
        name: "Tesla Roadster",
        image: images.tesla5,
        price: "200,000.00",
        numReviews: 120,
        rating: 4.3,
        numSolds: 987,
        navigate: "tesladetails",
        categoryId: "2",
        color: "#7A5548",
        quantity: 2,
        address: "567 Cedar St, Countryside",
        status: "Paid",
    },
    {
        id: "7",
        name: "Toyota Highlander",
        image: images.toyota4,
        price: "40,000.00",
        numReviews: 72,
        rating: 4.0,
        numSolds: 423,
        navigate: "toyotadetails",
        categoryId: "4",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "8",
        name: "Tesla Cybertruck",
        image: images.tesla6,
        price: "70,000.00",
        numReviews: 64,
        rating: 4.1,
        numSolds: 325,
        navigate: "tesladetails",
        categoryId: "2",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "9",
        name: "Mercedes-Benz S-Class Sedan",
        image: images.mercedes5,
        price: "100,000.00",
        numReviews: 120,
        rating: 4.3,
        numSolds: 987,
        navigate: "mercedesdetails",
        categoryId: "1",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    },
    {
        id: "10",
        name: "Toyota Tacoma",
        image: images.toyota5,
        price: "35,000.00",
        numReviews: 120,
        rating: 4.3,
        numSolds: 987,
        navigate: "toyotadetails",
        categoryId: "4",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Paid",
    }
]

export const cancelledOrders = [
    {
        id: "1",
        name: "Honda Civic",
        image: images.honda1,
        price: "22,000.00",
        numReviews: 143,
        rating: 4.5,
        numSolds: 9373,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#101010",
        quantity: 1,
        address: "123 Main St, Cityville",
        status: "Refunded",
    },
    {
        id: "2",
        name: "BMW X3 SUV",
        image: images.bmw3,
        price: "55,000.00",
        numReviews: 205,
        rating: 4.8,
        numSolds: 1689,
        navigate: "bmwdetails",
        categoryId: "3",
        color: "#7A5548",
        quantity: 2,
        address: "910 Elm St, Hamlet",
        status: "Refunded",
    },
    {
        id: "3",
        name: "Honda Accord",
        image: images.honda2,
        price: "24,000.00",
        numReviews: 98,
        rating: 4.2,
        numSolds: 562,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Refunded",
    },
    {
        id: "4",
        name: "Honda CR-V",
        image: images.honda3,
        price: "28,000.00",
        numReviews: 205,
        rating: 4.8,
        numSolds: 1689,
        navigate: "hondadetails",
        categoryId: "7",
        color: "#7A5548",
        quantity: 2,
        address: "321 Maple St, Suburbia",
        status: "Refunded",
    },
    {
        id: "5",
        name: "Mercedes-Benz GLC SUV",
        image: images.mercedes4,
        price: "60,000.00",
        numReviews: 72,
        rating: 4.0,
        numSolds: 423,
        navigate: "mercedesdetails",
        categoryId: "1",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Refunded",
    },
    {
        id: "6",
        name: "Tesla Roadster",
        image: images.tesla5,
        price: "200,000.00",
        numReviews: 120,
        rating: 4.3,
        numSolds: 987,
        navigate: "tesladetails",
        categoryId: "2",
        color: "#7A5548",
        quantity: 2,
        address: "567 Cedar St, Countryside",
        status: "Refunded",
    },
    {
        id: "7",
        name: "Toyota Highlander",
        image: images.toyota4,
        price: "40,000.00",
        numReviews: 72,
        rating: 4.0,
        numSolds: 423,
        navigate: "toyotadetails",
        categoryId: "4",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Refunded",
    },
    {
        id: "8",
        name: "Tesla Cybertruck",
        image: images.tesla6,
        price: "70,000.00",
        numReviews: 64,
        rating: 4.1,
        numSolds: 325,
        navigate: "tesladetails",
        categoryId: "2",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Refunded",
    },
    {
        id: "9",
        name: "Mercedes-Benz S-Class Sedan",
        image: images.mercedes5,
        price: "100,000.00",
        numReviews: 120,
        rating: 4.3,
        numSolds: 987,
        navigate: "mercedesdetails",
        categoryId: "1",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Refunded",
    },
    {
        id: "10",
        name: "Toyota Tacoma",
        image: images.toyota5,
        price: "35,000.00",
        numReviews: 120,
        rating: 4.3,
        numSolds: 987,
        navigate: "toyotadetails",
        categoryId: "4",
        color: "#7A5548",
        quantity: 2,
        address: "123 Main St, Cityville",
        status: "Refunded",
    }
]

export const notifications = [
    {
        id: "1",
        icon: icons.chat,
        title: "Product Inquiry from Kathryn",
        description: "Kathryn has sent you a message regarding a product inquiry. Tap to view.",
        date: "2024-01-16T04:52:06.501Z"
    },
    {
        id: "2",
        icon: icons.box,
        title: "Order Confirmation",
        description: "Congratulations! Your order has been successfully placed. Tap for details.",
        date: "2024-01-23T04:52:06.501Z"
    },
    {
        id: "3",
        icon: icons.chat,
        title: "New Product Announcement",
        description: "Exciting news! We have added new products to our collection. Tap to explore.",
        date: "2024-01-23T08:52:06.501Z"
    },
    {
        id: "4",
        icon: icons.discount,
        title: "Exclusive Discount Offer",
        description: "Enjoy a 20% discount on your next purchase! Limited time offer. Tap for details.",
        date: "2024-01-28T08:52:06.501Z"
    },
    {
        id: "5",
        icon: icons.chat,
        title: "New Feature Available",
        description: "Discover our latest feature that enhances your shopping experience. Tap to learn more.",
        date: "2024-01-29T08:52:06.501Z"
    },
    {
        id: "6",
        icon: icons.box,
        title: "Payment Method Linked",
        description: "Your payment method has been successfully linked to your account.",
        date: "2024-01-23T04:52:06.501Z"
    },
    {
        id: "7",
        icon: icons.chat,
        title: "Message from Julia",
        description: "Julia has sent you a message. Tap to read.",
        date: "2024-01-16T04:52:06.501Z"
    },
    {
        id: "8",
        icon: icons.chat,
        title: "Message from Joanna",
        description: "Joanna has sent you a message. Tap to read.",
        date: "2024-01-16T04:52:06.501Z"
    },
    {
        id: "9",
        icon: icons.chat,
        title: "Message from Lilia",
        description: "Lilia has sent you a message. Tap to read.",
        date: "2024-01-16T04:52:06.501Z"
    },
    {
        id: "10",
        icon: icons.box,
        title: "Account Setup Completed",
        description: "Congratulations! Your account setup has been completed successfully.",
        date: "2024-01-28T04:52:06.501Z"
    },
    {
        id: "11",
        icon: icons.discount,
        title: "Exclusive First Purchase Discount",
        description: "Receive a 50% discount on your first purchase! Limited time offer. Tap for details.",
        date: "2024-01-28T08:52:06.501Z"
    },
    {
        id: "12",
        icon: icons.chat,
        title: "Message from Mily",
        description: "Mily has sent you a message. Tap to read.",
        date: "2024-01-31T04:52:06.501Z"
    },
];
