
class InfoAboutUser extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            data: {}
        }
        $.get("/api" + window.location.pathname, (responseData) => this.setState({ data: responseData }));
    }

    render() {
        return (
            <div className="profile_wrapper">
                <div className="profile_icon">
                    <img href="#" />
                </div>
                <div className="profile_info">
                    <p>
                        Ник: {this.state.data.nickname}
                </p>
                    <p>
                        Email: {this.state.data.email}
                </p>
                    <p>
                        Почта подтверждена: {this.state.data.emailConfirmed}
                </p>
                    <p>
                        Номер телефона: {this.state.data.phoneNumber == undefined ? "no" : this.state.data.phoneNumber}
                </p>
                    <p>
                        Дата регистрации: {this.state.data.dateOfRegistration}
                </p>
                </div>
            </div>
        );
    }
}


ReactDOM.render(<InfoAboutUser />, document.getElementById("content"));

ReactDOM.render(<Seacher />, document.getElementById("seacher"));
ReactDOM.render(<Categories />, document.getElementById("listOfCategories"));
ReactDOM.render(<HandlerUserBlock />, document.getElementById("authorization"));
