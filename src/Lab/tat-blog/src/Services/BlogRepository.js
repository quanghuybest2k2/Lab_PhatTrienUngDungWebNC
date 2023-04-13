import { get_api, post_api } from "./Methods";

export function getPosts(
    keyword = "",
    pageSize = 10,
    pageNumber = 1,
    sortColumn = "",
    sortOrder = ""
) {
    return get_api(
        `https://localhost:44309/api/posts?keyword=${keyword}&PublishedOnly=true&UnPublished=false&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`
    );
}
export async function getPostsBySlug(slug = "") {
    return get_api(
        `https://localhost:44309/api/posts/byslug/${slug}?PageSize=10&PageNumber=1`
    );
}
export async function getPost(year, month, day, slug = "") {
    return get_api(
        `https://localhost:44309/api/posts?PageSize=1&PageNumber=1&PublishedOnly=true&UnPublished=false&Year=${year}&Month=${month}&Day=${day}&PostSlug=${slug}`
    );
}
export function getAuthors(
    name = "",
    pageSize = 10,
    pageNumber = 1,
    sortColumn = "",
    sortOrder = ""
) {
    return get_api(
        `https://localhost:44309/api/authors?name=${name}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`
    );
}
export function getFilter() {
    return get_api("https://localhost:44309/api/posts/get-filter");
}
export function getPostsFilter(
    keyword = "",
    authorId = "",
    categoryId = "",
    year = "",
    month = "",
    pageSize = 10,
    pageNumber = 1,
    sortColumn = "",
    sortOrder = ""
) {
    let url = new URL("https://localhost:44309/api/posts/get-posts-filter");
    keyword !== "" && url.searchParams.append("Keyword", keyword);
    authorId !== "" && url.searchParams.append("AuthorId", authorId);
    categoryId !== "" && url.searchParams.append("CategoryId", categoryId);
    year !== "" && url.searchParams.append("Year", year);
    month !== "" && url.searchParams.append("Month", month);
    sortColumn !== "" && url.searchParams.append("SortColumn", sortColumn);
    sortOrder !== "" && url.searchParams.append("SortOrder", sortOrder);
    url.searchParams.append("PageSize", pageSize);
    url.searchParams.append("PageNumber", pageNumber);
    return get_api(url.href);
}
export async function getPostById(id = 0) {
    if (id > 0) return get_api(`https://localhost:44309/api/posts/${id}`);
    return null;
}
export function addOrUpdatePost(formData) {
    return post_api("https://localhost:44309/api/posts", formData);
}
