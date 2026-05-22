import '../Styles/Global.css'
import '../Styles/ConversationList.css'

function ConversationsList() {
    return (
        <div className="conversationList">
            <input type="text" placeholder="Search conversations..." className="searchConversations" />
            <div className="profileList">
                <div className="profile">
                    <div className="profilePicture">
                        <img src="https://scontent-waw2-1.xx.fbcdn.net/v/t39.30808-6/689011211_2840587626289603_5600842067170356309_n.jpg?_nc_cat=108&ccb=1-7&_nc_sid=6ee11a&_nc_ohc=csj0HXqNPysQ7kNvwFH5hLV&_nc_oc=AdrW8UxI4mnd9HmI3sbaY4bCf_kSGq6Vd--Zf1SuRAvFnN06cxWcIiyfBgSrjO4M5SI&_nc_zt=23&_nc_ht=scontent-waw2-1.xx&_nc_gid=akP0FeQRiCBILdeN1__doQ&_nc_ss=7b2a8&oh=00_Af5hLjy5LLzLaHB9dETv7spOIPryRvWcJ-lYS0kUVGqZxw&oe=6A160699" alt="Awatar" className='contactAvatar'></img>
                    </div>
                    <b>Michał Buszka</b>
                </div>
                <div className="profile">
                    <div className="profilePicture">
                        <img src="https://scontent-waw2-1.xx.fbcdn.net/v/t39.30808-6/689011211_2840587626289603_5600842067170356309_n.jpg?_nc_cat=108&ccb=1-7&_nc_sid=6ee11a&_nc_ohc=csj0HXqNPysQ7kNvwFH5hLV&_nc_oc=AdrW8UxI4mnd9HmI3sbaY4bCf_kSGq6Vd--Zf1SuRAvFnN06cxWcIiyfBgSrjO4M5SI&_nc_zt=23&_nc_ht=scontent-waw2-1.xx&_nc_gid=akP0FeQRiCBILdeN1__doQ&_nc_ss=7b2a8&oh=00_Af5hLjy5LLzLaHB9dETv7spOIPryRvWcJ-lYS0kUVGqZxw&oe=6A160699" alt="Awatar" className='contactAvatar'></img>
                    </div>
                    <b>Michał Buszka</b>
                </div>
            </div>
        </div>
    );
}

export default ConversationsList;