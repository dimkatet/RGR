import React, { Component } from 'react';

export class Auth extends Component {
    static displayName = Auth.name;

    constructor(props) {
        super(props);
        this.state = { login: '', sText: '', s: 0, r: 0, n: 0, e: 0, authState: 0 };

        this.handleLogin = this.handleLogin.bind(this);
        this.handleKey = this.handleKey.bind(this);
        this.handleSend = this.handleSend.bind(this);
        this.authHandler = this.authHandler.bind(this);
    }

    handleLogin(event) {
        this.setState({ login: event.target.value });
    }


    handleKey(event) {
        this.setState({ sText: event.target.value });
    }

    async authHandler() {
        console.log('N:', this.state.n);
        var rItems = new Array();
        var rands = new Array();
        for (var i = 0; i < 30; i++) {
            const rand = 1 + Math.floor(Math.random() * (this.state.n - 1));
            var item = {
                n: (rand * rand) % this.state.n,
                stage: 1
            };
            rItems.push(item);
            rands.push(rand);
        }

        console.log('r: ', rItems);
        var response = await fetch('fiatshamir', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(rItems)
        });
        var data = await response.json();
        //this.setState({ e: data, authStage: 2 });
        console.log(data);

        var yItems = new Array();
        for (var i = 0; i < 30; i++) {
            const y = rands[i] * Math.pow(this.state.s, data[i]) % this.state.n;
            item = {
                n: y,
                stage: 2
            }
            yItems.push(item);
        }
        console.log('Y: ', yItems);
        response = await fetch('fiatshamir', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(yItems)
        });
        data = await response.json();
        this.setState({ authState: data[0] });
        console.log(this.state.authState);
    }

    async handleSend(event) {

        this.setState({ s: parseInt(this.state.sText) });
        const item = {
            login: this.state.login,
            V: 0,
            ID: 0
        }


        const response = await fetch('auth', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        });
        const data = await response.json();
        if(data == -1)
        {
            this.setState({ authState: data });
            return;
		}

        this.setState({ n: data, authStage: 1 });

        //console.log(this.state.authState);
        this.authHandler();
    }

    static renderAuthStatus(authStatus) {
        return (
            authStatus === 1
                ? <p> Успешно </p>
                : authStatus === -1
                    ? <p> Не верное имя пользоветеля или пароль </p>
                    : <p> </p>
        )
    }

    componentDidMount() {
    }

    render() {
        let authInf = Auth.renderAuthStatus(this.state.authState);
        return (
            <div>
                <h1 id="tabelLabel" >Вход</h1>
                <p>Введите ваш логин</p>
                <p>
                    <input type='text' size='40' id='textBox' value={this.state.login} onChange={this.handleLogin} />
                </p>
                <p>Введите ваш пароль</p>
                <p>
                    <input type='text' size='40' id='textBox2' value={this.state.sText} onChange={this.handleKey} />
                </p>
                <button className="btn btn-primary" onClick={this.handleSend}>Отправить</button>
                {authInf}
            </div>
        );
    }
}