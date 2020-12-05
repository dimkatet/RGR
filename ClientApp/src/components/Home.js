import React, { Component } from 'react';
//import { BigInt } from "BigInt.js";

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { regStatus: '', loading: true, login: '', s: 0, n: 0, nStatus: false};

        this.handleChange = this.handleChange.bind(this);
        this.handleSend = this.handleSend.bind(this);
    }

    componentDidMount() {
        this.getN()
    }

    handleChange(event) {
        this.setState({ login: event.target.value });
    }

    async getN() {
        console.log('getN');
        const response = await fetch('home', { method: 'GET' });
        const data = await response.json();
        this.setState({ n: data });
        this.setState({ nStatus: true })
        console.log(this.state.n);
    }

    async handleSend(event) {
        var c = 1n;
        const min = 1;
        const max = this.state.n;
        const rand = Math.floor(min + Math.random() * (max - min));
        const item = {
            Login: this.state.login,
            N: this.state.n,
            ID: rand * rand % this.state.n
        };

        const response = await fetch('home', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        });
        const data = await response.json();
        this.setState({ regStatus: data, loading: false, s: rand });
    }



    static renderRegStatus(regStatus, s) {
        return (
            regStatus === 0
                ? <p>Регистрация прошла успешно. Ваш ключ: {s}</p>
                : regStatus === -1
                    ? <p> Логин уже занят</p >
                    : regStatus === -2
                        ? <p>Не верное имя пользователя</p>
                        : <p> </p>
        );
    }

    render() {
        let contents = Home.renderRegStatus(this.state.regStatus, this.state.s);

        return (
            this.state.nStatus
                ?
                <div>
                    <h1 id="tabelLabel" >Регистрация</h1>
                    <p>Введите ваш логин</p>
                    <p><input type='text' size='40' id='textBox' value={this.state.login} onChange={this.handleChange} /></p>
                    <button className="btn btn-primary" onClick={this.handleSend}>Отправить</button>
                    {contents}
                </div>
                : <p> Загрузка...</p>
        );
    }
}
