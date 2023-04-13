import { get_api } from "./Methods";

export async function getCategories() {
    return get_api(
        `https://localhost:44309/api/categories?PageSize=10&PageNumber=1`
    );
}
export async function getFeaturedPosts() {
    return get_api(`https://localhost:44309/api/posts/featured/3`);
}
export async function getRandomPosts(limit) {
    return get_api(`https://localhost:44309/api/posts/random/${limit}`);
}
export async function getBestAuthors() {
    return get_api(`https://localhost:44309/api/authors/best/4`);
}
export async function getTagCloud() {
    return get_api(`https://localhost:44309/api/tags?PageSize=10&PageNumber=1`);
}
export async function getArchivesPosts() {
    return get_api(`https://localhost:44309/api/posts/archives/12`);
}
