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
            //https://localhost:44309/api/posts/featured/3
            `https://localhost:44309/api/posts/featured/3`
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
export async function getRandomPosts(limit) {
    try {
        const response = await axios.get(
            //https://localhost:44309/api/posts/random/5
            `https://localhost:44309/api/posts/random/${limit}`
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
export async function getBestAuthors() {
    try {
        const response = await axios.get(
            //https://localhost:44309/api/authors/best/4
            `https://localhost:44309/api/authors/best/4`
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
export async function getTagCloud() {
    try {
        const response = await axios.get(
            //https://localhost:44309/api/tags?PageSize=10&PageNumber=1
            `https://localhost:44309/api/tags?PageSize=10&PageNumber=1`
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
export async function getArchivesPosts() {
    try {
        const response = await axios.get(
            `https://localhost:44309/api/posts/archives/12`
        );
        const data = response.data;
        if (data.isSuccess) return data.result;
        else return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}