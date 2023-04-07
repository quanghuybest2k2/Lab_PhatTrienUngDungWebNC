import axios from "axios";

export async function getCategories() {
    try {
        const response = await axios.get(
            //https://localhost:44309/api/categories?ShowOnMenu=true&PageSize=10&PageNumber=1
            `https://localhost:44309/api/categories?PageSize=10&PageNumber=1`
        );
        const data = response.data;
        if (data.isSuccess) {
            return data.result;
        } else {
            return null;
        }
    } catch (error) {
        console.log("Error", error.message);
        return null;
    }
}
export async function getFeaturedPosts() {
    try {
        const response = await axios.get(
            //https://localhost:44309/api/posts/featured/2
            `https://localhost:44309/api/posts/featured/2`
        );
        const data = response.data;

        if (data.isSuccess) {
            return data.result;
        } else {
            return null;
        }
    } catch (error) {
        console.log("Error", error.message);
        return null;
    }
}
